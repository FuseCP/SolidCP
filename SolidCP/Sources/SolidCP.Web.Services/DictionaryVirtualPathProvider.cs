// davidegli

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Web;
using System.Reflection;
using System.Security.Permissions;
using System.Web.Caching;
using System.Web.Hosting;
using System.Resources;
using System.Threading;

namespace SolidCP.Web.Services
{

	/// <summary>
	/// A VirtualPathProvider that manages a Dictionary with virtual files.
	/// </summary>
	public class DictionaryVirtualPathProvider : System.Web.Hosting.VirtualPathProvider
	{

		/// <summary>
		/// The dictionary used to manage the virtual files.
		/// </summary>
		public class Dictionary : KeyedCollection<string, VirtualFileBase>
		{
			protected override string GetKeyForItem(VirtualFileBase item) { return item.VirtualPath.ToLower(); }
		}
		/// <summary>
		/// The Dictionary with the virtual files.
		/// </summary>
		public Dictionary FilesAndDirectories { get; protected set; }
		public DictionaryVirtualPathProvider() { FilesAndDirectories = new Dictionary(); }

		/// <summary>
		/// Returns the directory part for a path
		/// </summary>
		/// <param name="path">The path</param>
		/// <returns>Returns the directory part of the path.</returns>
		public static string DirectoryName(string path)
		{
			path = Paths.Absolute(path);
			int slash = path.LastIndexOf('/', path.Length > 2 ? path.Length - 2 : 0);
			return path.Substring(0, slash > 0 ? slash : 0);
		}
		/// <summary>
		/// Gets the VirtuaslFileBase object that is stored in the dirctionary or null, if the object was not found.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public virtual VirtualFileBase GetObject(string path)
		{
			if (string.IsNullOrEmpty(path)) return null;
			path = Services.Paths.Absolute(path).ToLower();

			lock (FilesAndDirectories)
			{
				if (FilesAndDirectories.Contains(path)) return FilesAndDirectories[path];
			}
			return null;
		}
		/// <summary>
		/// The previous VirtualPathProvider
		/// </summary>
		public new System.Web.Hosting.VirtualPathProvider Previous { get { return base.Previous; } }
		/// <summary>
		/// True if the file was found or exitst in a prevoius VirtualPathProvider.
		/// </summary>
		/// <param name="path">The path</param>
		/// <returns>True if file was found or exitst in a prevoius VirtualPathProvider.</returns>
		public override bool FileExists(string path)
		{
			var file = GetObject(path);
			bool res;
			return res = (file != null && file is System.Web.Hosting.VirtualFile || base.FileExists(path));
		}
		/// <summary>
		///  True if the directory was found or exitst in a prevoius VirtualPathProvider.
		/// </summary>
		/// <param name="path">The path</param>
		/// <returns>True if the driectory was found or exitst in a prevoius VirtualPathProvider.</returns>
		public override bool DirectoryExists(string path)
		{
			var spath = Paths.AddSlash(path);
			var file = GetObject(spath);
			return file != null && file is System.Web.Hosting.VirtualDirectory || base.DirectoryExists(path);
		}
		/// <summary>
		/// Gets a virtual file for the path or null if the file does not exist.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>The virtual file or null if the file does not exist.</returns>
		public override System.Web.Hosting.VirtualFile GetFile(string path)
		{
			System.Web.Hosting.VirtualFileBase file;
			if ((file = GetObject(path)) != null && file is System.Web.Hosting.VirtualFile) return (System.Web.Hosting.VirtualFile)file;
			else return base.GetFile(path);
		}
		/// <summary>
		/// Gets a virtual directory for the path or null if the directory does not exist.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>The virtual directory or null if the directory does not exist.</returns>
		public override System.Web.Hosting.VirtualDirectory GetDirectory(string path)
		{
			System.Web.Hosting.VirtualFileBase file;
			var spath = Paths.AddSlash(path);
			if ((file = GetObject(spath)) != null && file is System.Web.Hosting.VirtualDirectory) return (System.Web.Hosting.VirtualDirectory)file;
			else return base.GetDirectory(path);
		}
		/// <summary>
		/// Gets a cache dependency for the supplied path.
		/// </summary>
		/// <param name="virtualPath">The virtual path</param>
		/// <param name="virtualPathDependencies">The file's dependencies.</param>
		/// <param name="utcStart">The utc time for this cache dependency.</param>
		/// <returns>A cache dependency.</returns>
		public override CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
		{
			var file = GetObject(virtualPath);
			if (file != null && file is ICacheableObject)
			{
				return ((ICacheableObject)file).GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
			}
			else if (base.FileExists(virtualPath))
			{
				var deps = new List<string>();
				foreach (var dep in virtualPathDependencies.OfType<string>())
				{
					if (base.FileExists(dep)) deps.Add(dep);
				}
				return base.GetCacheDependency(virtualPath, deps, utcStart);
			}
			else return null;
		}
		/// <summary>
		/// Gets a hash for the virtual file.
		/// </summary>
		/// <param name="virtualPath">The file's path.</param>
		/// <param name="virtualPathDependencies">The file's dependencies.</param>
		/// <returns>The hash for the file.</returns>
		public override string GetFileHash(string virtualPath, IEnumerable virtualPathDependencies)
		{
			var file = GetObject(virtualPath);
			if (file != null && file is ICacheableObject)
			{
				return ((ICacheableObject)file).Hash ?? base.GetFileHash(virtualPath, virtualPathDependencies);
			}
			else
			{
				return base.GetFileHash(virtualPath, virtualPathDependencies);
			}
		}
		/// <summary>
		/// Adds a virtual directory to the dictionary.
		/// </summary>
		/// <param name="directory">The directory to add.</param>
		public virtual void Add(System.Web.Hosting.VirtualDirectory directory)
		{
			lock (FilesAndDirectories)
			{
				var path = directory.VirtualPath.ToLower();
				if (!FilesAndDirectories.Contains(path))
				{
					FilesAndDirectories.Add(directory);
					var dir = DirectoryName(path) + "/";
					if ((dir != "/" || path != "/") && !FilesAndDirectories.Contains(dir)) Add(new VirtualDirectory(dir, this));
				}
				else
				{
					try
					{
						FilesAndDirectories.Remove(path);
						FilesAndDirectories.Add(directory);
					}
					catch (Exception ex)
					{
						throw;
					}
				}
			}
		}
		/// <summary>
		/// Adds a virtual file to the dictionary.
		/// </summary>
		/// <param name="file">The file to add.</param>
		public virtual void Add(System.Web.Hosting.VirtualFile file)
		{
			lock (FilesAndDirectories)
			{
				var path = file.VirtualPath.ToLower();
				if (!FilesAndDirectories.Contains(path))
				{
					FilesAndDirectories.Add(file);
					var dir = DirectoryName(path) + "/";
					if (!FilesAndDirectories.Contains(dir)) Add(new VirtualDirectory(dir, this));
				}
				else
				{
					try
					{
						FilesAndDirectories.Remove(path);
						FilesAndDirectories.Add(file);
					}
					catch (Exception ex)
					{
						throw;
					}
				}
			}
		}
		/// <summary>
		/// Removes the virtual object at path from the dictionary.
		/// </summary>
		/// <param name="path">The path.</param>
		public virtual void Remove(string path) { FilesAndDirectories.Remove(path); }
		/// <summary>
		/// Updates the virtual object at path in the dictionary.
		/// </summary>
		/// <param name="path">The path.</param>
		public virtual void Update(string path) { }
		/// <summary>
		/// Registers the VirtualPathProvider using reflection. This also works for precompiled websites.
		/// </summary>
		[ReflectionPermission(SecurityAction.Demand)]
		public virtual void RegisterWithReflection()
		{
			// any settings about your VirtualPathProvider may go here.

			// we get the current instance of HostingEnvironment class. We can't create a new one
			// because it is not allowed to do so. An AppDomain can only have one HostingEnvironment
			// instance.
			HostingEnvironment hostingEnvironmentInstance = (HostingEnvironment)typeof(HostingEnvironment).InvokeMember("_theHostingEnvironment", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, null, null);
			if (hostingEnvironmentInstance == null)
				return;

			// we get the MethodInfo for RegisterVirtualPathProviderInternal method which is internal
			// and also static.
			MethodInfo mi = typeof(HostingEnvironment).GetMethod("RegisterVirtualPathProviderInternal", BindingFlags.NonPublic | BindingFlags.Static);
			if (mi == null)
				return;

			// finally we invoke RegisterVirtualPathProviderInternal method with one argument which
			// is the instance of our own VirtualPathProvider.
			mi.Invoke(hostingEnvironmentInstance, new object[] { this });
		}
		/// <summary>
		/// Registers the VirtualPathProvider. For precompiled websites use RegisterWithReflecion instead.
		/// </summary>
		public void Register()
		{
			// TODO configuration switch to use RegisterWithReflection. 
			HostingEnvironment.RegisterVirtualPathProvider(this);
		}

		static DictionaryVirtualPathProvider current = new DictionaryVirtualPathProvider();
		/// <summary>
		/// Gets the Current DictionaryVirtualPathPrrovider.
		/// </summary>
		public static DictionaryVirtualPathProvider Current { get => current; }
		/// <summary>
		/// Disposes the VirtualPathProvider.
		/// </summary>
		public void Shutdown() { }
		/// <summary>
		/// IAutostart Init. Registers the VirtualPathProvider.
		/// </summary>
		public static void Startup()
		{
			var p = Current;
			p.Register();
		}
	}
	/// <summary>
	/// A IHttpModule that rewrites paths to support default documents in VirtualPathProviders.
	/// </summary>
	public class VirtualPathRewriteDefault
	{
		/// <summary>
		/// Rewrites the http reuquest if it is refering to a virtual path that has a default.aspx file on it.
		/// This allows for support of default documents in DictionaryVirtualPathProvider.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The EventArgs.</param>
		public void RewriteDefault(object sender, EventArgs e)
		{
			var context = ((HttpApplication)sender).Context;
			string path;
			if (HostingEnvironment.ApplicationVirtualPath != "/")
			{
				path = VirtualPathUtility.AppendTrailingSlash(HostingEnvironment.ApplicationVirtualPath + context.Request.AppRelativeCurrentExecutionFilePath.Substring(1)) + "default.aspx";
			}
			else path = VirtualPathUtility.AppendTrailingSlash(context.Request.AppRelativeCurrentExecutionFilePath.Substring(1)) + "default.aspx";
			if (HostingEnvironment.VirtualPathProvider.FileExists(path)) context.RewritePath(path, false);
		}
		/// <summary>
		/// Maps static files to the StaticFileHandler to allow for non asp files in the DictionaryVirtualPathProvider.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The EventArgs.</param>
		public void MapStatic(object sender, EventArgs e)
		{ // TODO: does not work yet.
			var context = ((HttpApplication)sender).Context;
			string path;
			if (HostingEnvironment.ApplicationVirtualPath != "/")
			{
				path = HostingEnvironment.ApplicationVirtualPath + context.Request.AppRelativeCurrentExecutionFilePath.Substring(1);
			}
			else path = context.Request.AppRelativeCurrentExecutionFilePath.Substring(1);
			if (context.CurrentHandler == null && HostingEnvironment.VirtualPathProvider.FileExists(path))
			{
				//TODO				context.Handler = new System.Web.StaticFileHandler();
			}
		}
		/// <summary>
		/// The IHttpModule Init method.
		/// </summary>
		/// <param name="context">THe HttpApplication.</param>
		public void Init(HttpApplication context)
		{
			context.PostAuthorizeRequest += RewriteDefault;
			// TODO: map static.
			//context.PostMapRequestHandler += VirtualPathProvider.Current.MapStatic;
		}
		public void Dispose() { }
	}
	/// <summary>
	/// An interface for object that have cache dependencies.
	/// </summary>
	public interface ICacheableObject
	{
		/// <summary>
		/// A hash for the object.
		/// </summary>
		string Hash { get; }
		/// <summary>
		/// Gets a cache dependency for the object.
		/// </summary>
		/// <param name="virtualPath">The path.</param>
		/// <param name="virtualPathDependencies">Dependecies.</param>
		/// <param name="utcStart">Start time for the cache dependency.</param>
		/// <returns>A CacheDependency.</returns>
		CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart);
	}

	/*
	/// <summary>
	/// A cache dependency for virtual files.
	/// </summary>
	public class VirtualFileCacheDependency: CacheDependency {
		
		static System.IO.FileSystemWatcher Watcher = new FileSystemWatcher(Services.Paths.Map(Services.Domains.RootPath), "*.*");
		System.Web.Hosting.VirtualFileBase File;
		/// <summary>
		/// If the file changed, this method is called.
		/// </summary>
		public static void OnChanged(object sender, FileSystemEventArgs e) {
			if (e.ChangeType == WatcherChangeTypes.Created) {
				var dpath = Services.Paths.Unmap(e.FullPath);
				var path = Services.Paths.NoDomains(dpath);
				var vpath = Services.Paths.Virtual(path);
				var file = System.Web.Hosting.HostingEnvironment.VirtualPathProvider.GetDirectory(vpath);

				if (Services.Paths.FromVirtual(e.FullPath ) {
			var t = File.GetType();
			if (File is System.Web.Hosting.VirtualDirectory &&
				t != System.Web.Hosting.HostingEnvironment.VirtualPathProvider.GetDirectory(File.VirtualPath).GetType() ||
				File is System.Web.Hosting.VirtualFile &&
				t != System.Web.Hosting.HostingEnvironment.VirtualPathProvider.GetFile(File.VirtualPath).GetType()) {
					SetUtcLastModified(DateTime.UtcNow);
					NotifyDependencyChanged(sender, e);
			}
		}
		/// <summary>
		/// Creates a VirtualFileCacheDependency.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="files">Files the file depends on.</param>
		/// <param name="virtualPathDependecies">Paths the file depends on.</param>
		/// <param name="utcStart">The cache start time.</param>
		public VirtualFileCacheDependency(System.Web.Hosting.VirtualFileBase file, string[] files, string[] virtualPathDependecies, DateTime utcStart): base(files, virtualPathDependecies, utcStart) {
			this.File = file;
			Watcher.IncludeSubdirectories = Watcher.EnableRaisingEvents = true;
			Watcher.Renamed += OnChanged;
			Watcher.Deleted += OnChanged;
			Watcher.Created += OnChanged;
		}
	}*/

	public class SameFile : IEqualityComparer<System.Web.Hosting.VirtualFileBase>
	{
		public bool Equals(VirtualFileBase x, VirtualFileBase y)
		{
			return string.Compare(x.VirtualPath, y.VirtualPath, true) == 0;
		}

		public int GetHashCode(VirtualFileBase obj)
		{
			return obj.VirtualPath.GetHashCode();
		}
	}

	/// <summary>
	/// A virtual directory in a DirectoryVirtualPathProvider.
	/// </summary>
	public class VirtualDirectory : System.Web.Hosting.VirtualDirectory, ICacheableObject
	{
		/// <summary>
		/// The DictionaryVirtualPath provider of this directory.
		/// </summary>
		public virtual DictionaryVirtualPathProvider Provider { get; protected set; }
		/// <summary>
		/// Creates a VirtualDirecotry.
		/// </summary>
		/// <param name="path">The path of the directory.</param>
		/// <param name="provider">The DictionaryVirtualPath provider this directory belongs to.</param>
		public VirtualDirectory(string path, DictionaryVirtualPathProvider provider) : base(path) { Provider = provider; }
		/// <summary>
		/// Returns true if the file is a direct child of the path.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="path">The path.</param>
		/// <returns>Returns true if the file is a direct child of the path.</returns>
		public bool IsChild(string file, string path)
		{
			if (string.Compare(file, path, true) != 0 && file.StartsWith(path, StringComparison.OrdinalIgnoreCase))
			{
				var name = file.Substring(path.Length);
				return !name.Remove(name.Length - 1).Contains('/');
			}
			return false;
		}

		public bool LoadedOnly { get; set; }

		string PathWithSlash(bool dir)
		{
			var path = Services.Paths.AddSlash(VirtualPath);
			return path;
		}

		/// <summary>
		/// The children of this directory.
		/// </summary>
		public override IEnumerable Children
		{
			get
			{
				return Directories.OfType<System.Web.Hosting.VirtualFileBase>()
					.Concat(Files.OfType<System.Web.Hosting.VirtualFileBase>())
					.Distinct(new SameFile());
			}
		}
		/// <summary>
		/// The child directories of this diretory.
		/// </summary>
		public override IEnumerable Directories
		{
			get
			{
				var path = PathWithSlash(true);

				// return Provider.FilesAndDirectories.Where(f => f.Value is System.Web.Hosting.VirtualDirectory && IsChild(f.Key, path));
				var dirs = Provider.FilesAndDirectories
					.OfType<System.Web.Hosting.VirtualDirectory>()
					.Where(e => IsChild(e.VirtualPath, path));
				var vpath = Paths.RemoveSlash(VirtualPath);
				if (vpath == "") vpath = "/";
				if (Provider.Previous.DirectoryExists(vpath)) dirs = dirs.Concat(Provider.Previous.GetDirectory(vpath).Directories.OfType<System.Web.Hosting.VirtualDirectory>());
				return dirs
					.OrderBy(d => d.VirtualPath)
					.Distinct(new SameFile());
			}
		}
		/// <summary>
		/// The child files of this directory.
		/// </summary>
		public override IEnumerable Files
		{
			get
			{
				var path = PathWithSlash(false);
				var files = Provider.FilesAndDirectories
					.OfType<System.Web.Hosting.VirtualFile>()
					.Where(e => IsChild(e.VirtualPath, path));
				if (Provider.Previous.DirectoryExists(VirtualPath)) files = files.Concat(Provider.Previous.GetDirectory(VirtualPath).Files.OfType<System.Web.Hosting.VirtualFile>());
				return files
					.OrderBy(f => f.VirtualPath)
					.Distinct(new SameFile());
			}
		}
		/// <summary>
		/// A hash for this directory.
		/// </summary>
		public virtual string Hash { get { return Children.OfType<System.Web.Hosting.VirtualFileBase>().Sum(x => x.VirtualPath.GetHashCode()).ToString(); } }
		/// <summary>
		/// A cache dependency for this directory.
		/// </summary>
		/// <param name="virtualPath">The path.</param>
		/// <param name="virtualPathDependencies">The dependencies.</param>
		/// <param name="utcStart">The cache start time.</param>
		/// <returns>A cache dependency for this directory.</returns>
		public virtual CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
		{
			return null; // return new VirtualFileCacheDependency(this, new string[0], new string[0], utcStart);
		}
	}
	/// <summary>
	/// A virtual file in a DictionaryVirtualPathProvider.
	/// </summary>
	public abstract class VirtualFile : System.Web.Hosting.VirtualFile, ICacheableObject
	{
		/// <summary>
		/// The DirctionaryVirtualPathProvider this file belongs to.
		/// </summary>
		public virtual DictionaryVirtualPathProvider Provider { get; protected set; }

		public VirtualFile(string path, DictionaryVirtualPathProvider provider) : base(path) { Provider = provider; }
		/// <summary>
		/// A hash for this file.
		/// </summary>
		public virtual string Hash { get { return null; } }
		/// <summary>
		/// Gets a cache dependency for this file.
		/// </summary>
		/// <param name="virtualPath">The path.</param>
		/// <param name="virtualPathDependencies">All dependencies.</param>
		/// <param name="utcStart">The cache start time.</param>
		/// <returns>A cache dependency.</returns>
		public virtual System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart) { return null; }
	}

	/// <summary>
	/// A virtual file that is stored in an assembly resource.
	/// </summary>
	public class ResourceVirtualFile : VirtualFile
	{
		/// <summary>
		/// The assembly that contains the file.
		/// </summary>
		public Assembly Assembly { get; protected set; }
		/// <summary>
		/// The name of the resource.
		/// </summary>
		public string ResourceName { get; protected set; }
		/// <summary>
		/// The resourceset that contains the resource.
		/// </summary>
		public ResourceSet ResourceSet { get; protected set; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="provider"></param>
		/// <param name="a"></param>
		/// <param name="set"></param>
		/// <param name="resourceName"></param>
		public ResourceVirtualFile(string path, DictionaryVirtualPathProvider provider, Assembly a, ResourceSet set, string resourceName) : base(path, provider)
		{
			Assembly = a; ResourceSet = set; ResourceName = resourceName;
		}

		class StreamWrapper : Stream
		{
			public Stream BaseStream { get; set; }
			public override long Position { get; set; }

			public StreamWrapper(Stream baseStream) : base() { BaseStream = baseStream; Position = 0; }

			void SeekPosition() { if (Position != BaseStream.Position) BaseStream.Seek(Position, SeekOrigin.Begin); }
			public override bool CanRead { get { return true; } }
			public override bool CanSeek { get { return true; } }
			public override bool CanTimeout { get { return false; } }
			public override bool CanWrite { get { return false; } }
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state) { Monitor.Enter(BaseStream); SeekPosition(); return BaseStream.BeginRead(buffer, offset, count, callback, state); }
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) { throw new NotImplementedException(); }
			public override int EndRead(IAsyncResult asyncResult) { var res = BaseStream.EndRead(asyncResult); Position = BaseStream.Position; Monitor.Exit(BaseStream); return res; }
			public override void EndWrite(IAsyncResult asyncResult) { throw new NotSupportedException(); }
			public override void Flush() { BaseStream.Flush(); }
			public override long Length { get { return BaseStream.Length; } }
			public override int Read(byte[] buffer, int offset, int count) { lock (BaseStream) { SeekPosition(); var res = BaseStream.Read(buffer, offset, count); Position = BaseStream.Position; return res; } }
			public override int ReadByte() { lock (BaseStream) { SeekPosition(); var res = BaseStream.ReadByte(); Position = BaseStream.Position; return res; } }
			public override long Seek(long offset, SeekOrigin origin) { lock (BaseStream) return Position = BaseStream.Seek(offset, origin); }
			public override void SetLength(long value) { throw new NotImplementedException(); }
			public override void Write(byte[] buffer, int offset, int count) { throw new NotImplementedException(); }
			public override void WriteByte(byte value) { throw new NotImplementedException(); }
		}

		public override Stream Open()
		{
			try
			{
				//var ext = Paths.Extension(ResourceName);
				Stream stream = (Stream)ResourceSet.GetObject(ResourceName);
				/*if (ext == "aspx" || ext == "ascx" || ext == "asmx" || ext == "ashx") {
					var buf = new MemoryStream();
					var w = new StreamWriter(buf);
					w.Write("<%@ Assembly Name=\"");
					w.Write(Assembly.DisplayName());
					w.Write("\" %>");
					w.Flush();
					stream.CopyTo(buf);
					buf.Seek(0, SeekOrigin.Begin);
					//stream = buf;REgister
					return buf;
				}*/

				return new StreamWrapper(stream);
				//return ms;
			}
			catch (Exception ex)
			{
				throw;
			}
			return null;
		}

		public override string Hash
		{
			get
			{
				var hash = Assembly.FullName + "; " + File.GetLastWriteTimeUtc(Assembly.Location).ToFileTime().ToString() +
					((HostingEnvironment.VirtualPathProvider.GetFile(VirtualPath).GetType() == typeof(ResourceVirtualFile)) ? ".resource" : ".user");
				return hash;
			}
		}

		public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
		{
			return null;
		}
	}

	//TODO securtiy hole web.config (deny in web.config has no effect).
	public class VirtualFiles
	{

		const string StandardVirtualPath = "app_virtual";
		public static readonly string[] StandardVirtualPaths = new string[] { "silversite", "app_browsers", "app_themes", "csharp" };

		public static readonly List<string> VirtualPaths = new List<string>(StandardVirtualPaths);

		// HashSet<AssemblyName> custom = new HashSet<AssemblyName>();
		// HashSet<AssemblyName> loaded = new HashSet<AssemblyName>();

		public static void AddFile(string path, string key, DictionaryVirtualPathProvider provider, Assembly a, ResourceSet set)
		{
			try
			{
				var file = new ResourceVirtualFile(Services.Paths.Absolute(path), provider, a, set, key);
				DictionaryVirtualPathProvider.Current.Add(file);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		/*
		public void LoadAssembly(Assembly a) {
			lock (a) {
				if (!loaded.Contains(a.GetName()) && !a.IsDynamic) {

					// load references first
					foreach (var name in a.GetReferencedAssemblies().Where(n => custom.Contains(n))) {
						if (!loaded.Contains(name)) {
							var asm = System.Reflection.Assembly.Load(name);
							LoadAssembly(asm);
						}
					}

					// load linked resource files into DictionaryVirtualPathProvider.
					try {
						var provider = DictionaryVirtualPathProvider.Current;
						var resources = a.GetManifestResourceNames();
						var linkedresources = resources.Where(r => r.EndsWith(".g.resources")).OrderBy(r => r.Length).FirstOrDefault();
						if (linkedresources != null) {
							var res = a.GetManifestResourceStream(linkedresources);
							var set = new ResourceSet(res);
							var e = set.GetEnumerator();
							while (e.MoveNext()) {
								var key = ((string)e.Key).ToLower();
								if (key.StartsWith(StandardVirtualPath + "/")) {
									var path = "~" + key.Substring(StandardVirtualPath.Length);
									AddFile(path, key, provider, a, set);
								} else {
									foreach (var vp in VirtualPaths) {
										if (key.StartsWith(vp + "/")) {
											AddFile("~/" + key, key, provider, a, set);
										}
									}
								}
							}
						}
					} catch (NotSupportedException) { }

					loaded.Add(a.GetName());
				}
			}
		} */

		public void AssemblyLoaded(object sender, AssemblyLoadEventArgs args)
		{
			var a = args.LoadedAssembly;
			try
			{
				var provider = DictionaryVirtualPathProvider.Current;
				var resources = a.GetManifestResourceNames();
				var linkedresources = resources.Where(r => r.EndsWith(".g.resources")).OrderBy(r => r.Length).FirstOrDefault();
				if (linkedresources != null)
				{
					var res = a.GetManifestResourceStream(linkedresources);
					var set = new ResourceSet(res);
					var e = set.GetEnumerator();
					while (e.MoveNext())
					{
						var key = ((string)e.Key).ToLower();
						if (true || key.StartsWith(StandardVirtualPath + "/"))
						{
							var path = "~" + key.Substring(StandardVirtualPath.Length);
							AddFile(path, key, provider, a, set);
						}
						else
						{
							foreach (var vp in VirtualPaths)
							{
								if (key.StartsWith(vp + "/"))
								{
									AddFile("~/" + key, key, provider, a, set);
								}
							}
						}
					}
				}
			}
			catch (NotSupportedException) { }
		}

		//public static VirtualFiles Current { get; private set; }

		// static object Lock = new object();

		public void Startup()
		{

			//Current = this;

			//var provider = DictionaryVirtualPathProvider.Current;

			//custom = new HashSet<AssemblyName>(Services.Types.CustomAssemblies.Select(a => a.GetName()));
		}
		public void Shutdown() { }

	}
}