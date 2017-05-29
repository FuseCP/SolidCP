// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.Mail
{
	public class Tree
	{
		private TreeNodeCollection childNodes;

		public TreeNodeCollection ChildNodes
		{
			get { return childNodes; }
		}

		public Tree()
		{
			childNodes = new TreeNodeCollection();
		}

		public void Serialize(StringBuilder builder)
		{
			foreach(TreeNode node in childNodes.ToArray())
				node.Serialize(builder);
		}
	}

	public class TreeNodeCollection : ICollection<TreeNode>
	{
		private List<TreeNode> _collection;
		private Dictionary<string, TreeNode> _searchIndex;

		public TreeNode this[string keyName]
		{
			get
			{
				if (_searchIndex.ContainsKey(keyName))
					return _searchIndex[keyName];

				return null;
			}
		}

		public TreeNodeCollection()
		{
			_collection = new List<TreeNode>();
			_searchIndex = new Dictionary<string, TreeNode>();
		}

		public void Add(TreeNode node)
		{
			string keyName = node.NodeName;

			_collection.Add(node);

			if (keyName != TreeNode.Unnamed)
				_searchIndex.Add(keyName, node);
		}

		public TreeNode[] ToArray()
		{
			return _collection.ToArray();
		}

		#region ICollection<TreeNode> Members


		public void Clear()
		{
			_collection.Clear();
		}

		public bool Contains(TreeNode item)
		{
			return _collection.Contains(item);
		}

		public void CopyTo(TreeNode[] array, int arrayIndex)
		{
			_collection.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _collection.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(TreeNode item)
		{
			return _collection.Remove(item);
		}

		#endregion

		#region IEnumerable<TreeNode> Members

		public IEnumerator<TreeNode> GetEnumerator()
		{
			return _collection.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _collection.GetEnumerator();
		}

		#endregion
	}

	public class TreeNode
	{
		private Tree root;
		private TreeNode parent;
		private TreeNodeCollection childNodes;

		private string nodeName;
		private string nodeValue;

		public const string Unnamed = "-";

		public TreeNodeCollection ChildNodes
		{
			get
			{
				EnsureChildControlsCreated();

				return childNodes;
			}
		}

		public bool IsUnnamed
		{
			get { return nodeName == Unnamed; }
		}

		public Tree Root
		{
			get { return root;  }
			set { root = value; }
		}

		public TreeNode Parent
		{
			get { return parent;  }
			set { parent = value; }
		}

		public string NodeName
		{
			get { return nodeName;	}
			set { nodeName = value; }
		}

		public string NodeValue
		{
			get { return nodeValue;  }
			set { nodeValue = value; }
		}

		public string this[string keyName]
		{
			get
			{
				EnsureChildControlsCreated();

				TreeNode keyNode = childNodes[keyName];
				if (keyNode != null)
					return keyNode.NodeValue;

				return null;
			}
			set
			{
				EnsureChildControlsCreated();

				TreeNode keyNode = childNodes[keyName];
				if (keyNode == null)
				{
					keyNode = new TreeNode(this);
					keyNode.NodeName = keyName;
					childNodes.Add(keyNode);
				}

				keyNode.NodeValue = value;
			}
		}

		public TreeNode(TreeNode parent) : this()
		{
			this.parent = parent;
		}

		public TreeNode()
		{
			this.nodeName = Unnamed;
		}

		private void EnsureChildControlsCreated()
		{
			if (childNodes == null)
				childNodes = new TreeNodeCollection();
		}

		public virtual void Serialize(StringBuilder builder)
		{
			if (childNodes != null)
			{
				builder.AppendLine(string.Concat("{ ", nodeName));

				foreach (TreeNode node in childNodes)
					node.Serialize(builder);

				builder.AppendLine("}");
			}
			else
			{
				builder.AppendLine(string.Concat(nodeName, "=", nodeValue));
			}
		}
	}
}
