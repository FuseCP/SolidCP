using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SolidCP.EnterpriseServer.Data.Entities;
using SolidCP.EnterpriseServer.Data.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

#if NetCore
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
#elif NetFX
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Data.Entity.Validation;
using MC=System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
#endif


namespace SolidCP.EnterpriseServer.Data
{
#if NetCore
	public abstract class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
	{
		public const bool IsCore = true;
		public const bool IsNetFX = false;
#elif NetFX
	public abstract class EntityTypeConfiguration<TEntity> : MC.EntityTypeConfiguration<TEntity> where TEntity : class {
		public const bool IsCore = false;
		public const bool IsNetFX = true;
#else
	public abstract class EntityTypeConfiguration<TEntity> {
		public const bool IsCore = false;
		public const bool IsNetFX = false;
		public void ApplyConfiguration(EntityTypeConfiguration<TEntity> configuration) { }
#endif
		public DbType DbType { get; set; } = DbType.Unknown;
		public bool InitSeedData { get; set; } = false;
		public bool IsMsSql => DbType == DbType.SqlServer;
		public bool IsMySql => DbType == DbType.MySql;
		public bool IsMariaDb => DbType == DbType.MariaDb;
		public bool IsSqlite => IsSqliteCore || IsSqliteFX;
		public bool IsSqliteCore => DbType == DbType.Sqlite;
		public bool IsSqliteFX => DbType == DbType.SqliteFX;
		public bool IsPostgreSql => DbType == DbType.PostgreSql;

		public EntityTypeConfiguration() : base() { }
		public EntityTypeConfiguration(DbType dbType, bool initSeedData = false) : this() { DbType = dbType; InitSeedData = initSeedData; }

#if NetCore
		EntityTypeBuilder<TEntity> Model = null;
		public void Configure(EntityTypeBuilder<TEntity> model) { Model = model; Configure(); }
		public EntityTypeBuilder<TEntity> Core => Model;
#elif NetFX
		public MC.EntityTypeConfiguration<TEntity> NetFX => (MC.EntityTypeConfiguration<TEntity>)this;
#endif
		public abstract void Configure();

		//
		// Summary:
		//     Adds or updates an annotation on the entity type. If an annotation with the key
		//     specified in annotation already exists its value will be updated.
		//     On NET Core this calls HasTableAnnotation.
		// Parameters:
		//   annotation:
		//     The key of the annotation to be added or updated.
		//
		//   value:
		//     The value to be stored in the annotation.
		//
		// Returns:
		//     The same typeBuilder instance so that multiple configuration calls can be chained.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> HasAnnotation(string annotation, object? value) => Model.HasAnnotation(annotation, value);
#endif

		//
		// Summary:
		//     Sets the base type of this entity type in an inheritance hierarchy.
		//
		// Parameters:
		//   name:
		//     The name of the base type or null to indicate no base type.
		//     Only implemented for NET Core.
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> HasBaseType(string? name) => Model.HasBaseType(name);
#endif

		//
		// Summary:
		//     Sets the base type of this entity type in an inheritance hierarchy.
		//     Only implemented for NET Core.
		// Parameters:
		//   entityType:
		//     The base type or null to indicate no base type.
		//
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> HasBaseType(Type? entityType) => Model.HasBaseType(entityType);
#endif

		//
		// Summary:
		//     Sets the base type of this entity type in an inheritance hierarchy.
		//     Only implemented for NET Core.
		// Type parameters:
		//   TBaseType:
		//     The base type or null to indicate no base type.
		//
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> HasBaseType<TBaseType>() => Model.HasBaseType<TBaseType>();
#endif

		//
		// Summary:
		//     Sets the properties that make up the primary key for this entity type.
		//
		// Parameters:
		//   keyExpression:
		//     A lambda expression representing the primary key property(s) (blog => blog.Url).
		//
		//
		//     If the primary key is made up of multiple properties then specify an anonymous
		//     type including the properties (post => new { post.Title, post.BlogId }).
		//
		// Returns:
		//     An object that can be used to configure the primary key.
#if NetCore
		public virtual KeyBuilder HasKey(Expression<Func<TEntity, object?>> keyExpression) => Model.HasKey(keyExpression);
#elif NetFX
		public new virtual PrimaryKeyIndexConfiguration HasKey(Expression<Func<TEntity, object?>> keyExpression)
		{
			PrimaryKeyIndexConfiguration conf = null;
			HasKey(keyExpression, key => conf = key);
			return conf;
		}
#endif

		//
		// Summary:
		//     Sets the properties that make up the primary key for this entity type.
		//     Only implemented for NET Core
		//
		// Parameters:
		//   propertyNames:
		//     The names of the properties that make up the primary key.
		//
		// Returns:
		//     An object that can be used to configure the primary key.
#if NetCore
		public virtual KeyBuilder<TEntity> HasKey(params string[] propertyNames ) => Model.HasKey(propertyNames);
#endif

		//
		// Summary:
		//     Creates an alternate key in the model for this entity type if one does not already
		//     exist over the specified properties. This will force the properties to be read-only.
		//     Use Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder`1.HasIndex(System.String[])
		//     or Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder`1.HasIndex(System.Linq.Expressions.Expression{System.Func{`0,System.Object}})
		//     to specify uniqueness in the model that does not force properties to be read-only.
		//
		//
		// Parameters:
		//   keyExpression:
		//     A lambda expression representing the key property(s) (blog => blog.Url).
		//
		//     If the key is made up of multiple properties then specify an anonymous type including
		//     the properties (post => new { post.Title, post.BlogId }).
		//
		// Returns:
		//     An object that can be used to configure the key.
#if NetCore
		public virtual KeyBuilder<TEntity> HasAlternateKey(Expression<Func<TEntity, object?>> keyExpression) => Model.HasAlternateKey(keyExpression);
#endif

		//
		// Summary:
		//     Creates an alternate key in the model for this entity type if one does not already
		//     exist over the specified properties. This will force the properties to be read-only.
		//     Use Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder`1.HasIndex(System.String[])
		//     or Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder`1.HasIndex(System.Linq.Expressions.Expression{System.Func{`0,System.Object}})
		//     to specify uniqueness in the model that does not force properties to be read-only.
		//
		//
		// Parameters:
		//   propertyNames:
		//     The names of the properties that make up the key.
		//
		// Returns:
		//     An object that can be used to configure the key.
#if NetCore
		public virtual KeyBuilder<TEntity> HasAlternateKey(params string[] propertyNames) => Model.HasAlternateKey(propertyNames);
#endif

		//
		// Summary:
		//     Configures the entity type to have no keys. It will only be usable for queries.
		//
		//
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> HasNoKey() => Model.HasNoKey();
#endif

		//
		// Summary:
		//     Returns an object that can be used to configure a property of the entity type.
		//     If the specified property is not already part of the model, it will be added.
		//
		//
		// Parameters:
		//   propertyExpression:
		//     A lambda expression representing the property to be configured ( blog => blog.Url).
		//
		//
		// Returns:
		//     An object that can be used to configure the property.
#if NetCore
		public virtual PropertyBuilder<TProperty> Property<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression) => Model.Property(propertyExpression);
#endif

		//
		// Summary:
		//     Returns an object that can be used to configure a property of the entity type
		//     where that property represents a collection of primitive values, such as strings
		//     or integers.
		//
		// Parameters:
		//   propertyExpression:
		//     A lambda expression representing the property to be configured ( blog => blog.Url).
		//
		//
		// Returns:
		//     An object that can be used to configure the property.
#if NetCore
		public virtual PrimitiveCollectionBuilder<TProperty> PrimitiveCollection<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression) => Model.PrimitiveCollection(propertyExpression);
#endif

		//
		// Summary:
		//     Configures a complex property of the entity type. If no property with the given
		//     name exists, then a new property will be added.
		//
		// Parameters:
		//   propertyName:
		//     The name of the property to be configured.
		//
		//   buildAction:
		//     An action that performs configuration of the property.
		//
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
		//
		//
		// Remarks:
		//     When adding a new property with this overload the property name must match the
		//     name of a CLR property or field on the complex type. This overload cannot be
		//     used to add a new shadow state complex property.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> ComplexProperty(string propertyName, Action<ComplexPropertyBuilder> buildAction) => Model.ComplexProperty(propertyName, buildAction);
#endif

		//
		// Summary:
		//     Configures a complex property of the entity type. If no property with the given
		//     name exists, then a new property will be added.
		//
		// Parameters:
		//   propertyName:
		//     The name of the property to be configured.
		//
		//   buildAction:
		//     An action that performs configuration of the property.
		//
		// Type parameters:
		//   TProperty:
		//     The type of the property to be configured.
		//
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
		//
		//
		// Remarks:
		//     When adding a new property, if a property with the same name exists in the complex
		//     class then it will be added to the model. If no property exists in the complex
		//     class, then a new shadow state complex property will be added. A shadow state
		//     property is one that does not have a corresponding property in the complex class.
		//     The current value for the property is stored in the Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker
		//     rather than being stored in instances of the complex class.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> ComplexProperty<TProperty>(string propertyName, Action<ComplexPropertyBuilder<TProperty>> buildAction) => Model.ComplexProperty(propertyName, buildAction);
#endif

		//
		// Summary:
		//     Configures a complex property of the entity type. If no property with the given
		//     name exists, then a new property will be added.
		//
		// Parameters:
		//   propertyName:
		//     The name of the property to be configured.
		//
		//   complexTypeName:
		//     The name of the complex type.
		//
		//   buildAction:
		//     An action that performs configuration of the property.
		//
		// Type parameters:
		//   TProperty:
		//     The type of the property to be configured.
		//
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
		//
		//
		// Remarks:
		//     When adding a new property, if a property with the same name exists in the complex
		//     class then it will be added to the model. If no property exists in the complex
		//     class, then a new shadow state complex property will be added. A shadow state
		//     property is one that does not have a corresponding property in the complex class.
		//     The current value for the property is stored in the Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker
		//     rather than being stored in instances of the complex class.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> ComplexProperty<TProperty>(string propertyName, string complexTypeName, Action<ComplexPropertyBuilder<TProperty>> buildAction) => Model.ComplexProperty(propertyName, complexTypeName, buildAction);
#endif

		//
		// Summary:
		//     Returns an object that can be used to configure a complex property of the complex
		//     type. If no property with the given name exists, then a new property will be
		//     added.
		//
		// Parameters:
		//   propertyType:
		//     The type of the property to be configured.
		//
		//   propertyName:
		//     The name of the property to be configured.
		//
		//   buildAction:
		//     An action that performs configuration of the property.
		//
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
		//
		//
		// Remarks:
		//     When adding a new complex property, if a property with the same name exists in
		//     the complex class then it will be added to the model. If no property exists in
		//     the complex class, then a new shadow state complex property will be added. A
		//     shadow state property is one that does not have a corresponding property in the
		//     complex class. The current value for the property is stored in the Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker
		//     rather than being stored in instances of the complex class.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> ComplexProperty(Type propertyType, string propertyName, Action<ComplexPropertyBuilder> buildAction) => Model.ComplexProperty(propertyType, propertyName, buildAction);
#endif

		//
		// Summary:
		//     Returns an object that can be used to configure a complex property of the complex
		//     type. If no property with the given name exists, then a new property will be
		//     added.
		//
		// Parameters:
		//   propertyType:
		//     The type of the property to be configured.
		//
		//   propertyName:
		//     The name of the property to be configured.
		//
		//   complexTypeName:
		//     The name of the complex type.
		//
		//   buildAction:
		//     An action that performs configuration of the property.
		//
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
		//
		//
		// Remarks:
		//     When adding a new complex property, if a property with the same name exists in
		//     the complex class then it will be added to the model. If no property exists in
		//     the complex class, then a new shadow state complex property will be added. A
		//     shadow state property is one that does not have a corresponding property in the
		//     complex class. The current value for the property is stored in the Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker
		//     rather than being stored in instances of the complex class.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> ComplexProperty(Type propertyType, string propertyName, string complexTypeName, Action<ComplexPropertyBuilder> buildAction) => ComplexProperty(propertyType, propertyName, complexTypeName, buildAction);
#endif

		//
		// Summary:
		//     Returns an object that can be used to configure a complex property of the entity
		//     type. If the specified property is not already part of the model, it will be
		//     added.
		//
		// Parameters:
		//   propertyExpression:
		//     A lambda expression representing the property to be configured ( blog => blog.Url).
		//
		//
		// Returns:
		//     An object that can be used to configure the complex property.
#if NetCore
		public virtual ComplexPropertyBuilder<TProperty> ComplexProperty<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression) => Model.ComplexProperty(propertyExpression);
#endif

		//
		// Summary:
		//     Returns an object that can be used to configure a complex property of the entity
		//     type. If the specified property is not already part of the model, it will be
		//     added.
		//
		// Parameters:
		//   propertyExpression:
		//     A lambda expression representing the property to be configured ( blog => blog.Url).
		//
		//
		//   complexTypeName:
		//     The name of the complex type.
		//
		// Returns:
		//     An object that can be used to configure the complex property.
#if NetCore
		public virtual ComplexPropertyBuilder<TProperty> ComplexProperty<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, string complexTypeName) => Model.ComplexProperty(propertyExpression, complexTypeName);
#endif

		//
		// Summary:
		//     Configures a complex property of the entity type. If the specified property is
		//     not already part of the model, it will be added.
		//
		// Parameters:
		//   propertyExpression:
		//     A lambda expression representing the property to be configured ( blog => blog.Url).
		//
		//
		//   buildAction:
		//     An action that performs configuration of the property.
		//
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> ComplexProperty<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, Action<ComplexPropertyBuilder<TProperty>> buildAction) => Model.ComplexProperty(propertyExpression, buildAction);
#endif

		//
		// Summary:
		//     Configures a complex property of the entity type. If the specified property is
		//     not already part of the model, it will be added.
		//
		// Parameters:
		//   propertyExpression:
		//     A lambda expression representing the property to be configured ( blog => blog.Url).
		//
		//
		//   complexTypeName:
		//     The name of the complex type.
		//
		//   buildAction:
		//     An action that performs configuration of the property.
		//
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> ComplexProperty<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, string complexTypeName, Action<ComplexPropertyBuilder<TProperty>> buildAction) => Model.ComplexProperty(propertyExpression, complexTypeName, buildAction);
#endif

		//
		// Summary:
		//     Returns an object that can be used to configure an existing navigation property
		//     of the entity type. It is an error for the navigation property not to exist.
		//
		//
		// Parameters:
		//   navigationExpression:
		//     A lambda expression representing the navigation property to be configured ( blog
		//     => blog.Posts).
		//
		// Type parameters:
		//   TNavigation:
		//     The target entity type.
		//
		// Returns:
		//     An object that can be used to configure the navigation property.
#if NetCore
		public virtual NavigationBuilder<TEntity, TNavigation> Navigation<TNavigation>(Expression<Func<TEntity, TNavigation?>> navigationExpression) where TNavigation : class => Model.Navigation(navigationExpression);
#endif

		//
		// Summary:
		//     Returns an object that can be used to configure an existing navigation property
		//     of the entity type. It is an error for the navigation property not to exist.
		//
		//
		// Parameters:
		//   navigationExpression:
		//     A lambda expression representing the navigation property to be configured ( blog
		//     => blog.Posts).
		//
		// Type parameters:
		//   TNavigation:
		//     The target entity type.
		//
		// Returns:
		//     An object that can be used to configure the navigation property.
#if NetCore
		public virtual NavigationBuilder<TEntity, TNavigation> Navigation<TNavigation>(Expression<Func<TEntity, IEnumerable<TNavigation>?>> navigationExpression) where TNavigation : class => Model.Navigation(navigationExpression);
#endif

		//
		// Summary:
		//     Excludes the given property from the entity type. This method is typically used
		//     to remove properties or navigations from the entity type that were added by convention.
		//
		//
		// Parameters:
		//   propertyExpression:
		//     A lambda expression representing the property to be ignored (blog => blog.Url).
#if NetCore
		public virtual EntityTypeBuilder<TEntity> Ignore(Expression<Func<TEntity, object?>> propertyExpression) => Model.Ignore(propertyExpression);
#endif

		//
		// Summary:
		//     Excludes the given property from the entity type. This method is typically used
		//     to remove properties or navigations from the entity type that were added by convention.
		//
		//
		// Parameters:
		//   propertyName:
		//     The name of the property to be removed from the entity type.
#if NetCore
		public new virtual EntityTypeBuilder<TEntity> Ignore(string propertyName) => Model.Ignore(propertyName);
#endif

		//
		// Summary:
		//     Specifies a LINQ predicate expression that will automatically be applied to any
		//     queries targeting this entity type.
		//
		// Parameters:
		//   filter:
		//     The LINQ predicate expression.
		//
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
#if NetCore
		public new virtual EntityTypeBuilder<TEntity> HasQueryFilter(LambdaExpression? filter) => Model.HasQueryFilter(filter);
#endif

		//
		// Summary:
		//     Specifies a LINQ predicate expression that will automatically be applied to any
		//     queries targeting this entity type.
		//
		// Parameters:
		//   filter:
		//     The LINQ predicate expression.
		//
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> HasQueryFilter(Expression<Func<TEntity, bool>>? filter) => Model.HasQueryFilter(filter);
#endif

		//
		// Summary:
		//     Configures a query used to provide data for a keyless entity type.
		//
		// Parameters:
		//   query:
		//     The query that will provide the underlying data for the keyless entity type.
		//
		//
		// Returns:
		//     The same builder instance so that multiple calls can be chained.
#if NetCore
		[Obsolete("Use InMemoryEntityTypeBuilderExtensions.ToInMemoryQuery")]
		public virtual EntityTypeBuilder<TEntity> ToQuery(Expression<Func<IQueryable<TEntity>>> query) => Model.ToQuery(query);
#endif

		//
		// Summary:
		//     Configures an unnamed index on the specified properties. If there is an existing
		//     index on the given list of properties, then the existing index will be returned
		//     for configuration.
		//
		// Parameters:
		//   indexExpression:
		//     A lambda expression representing the property(s) to be included in the index
		//     (blog => blog.Url).
		//
		//     If the index is made up of multiple properties then specify an anonymous type
		//     including the properties (post => new { post.Title, post.BlogId }).
		//
		// Returns:
		//     An object that can be used to configure the index.
#if NetCore
		public virtual IndexBuilder<TEntity> HasIndex(Expression<Func<TEntity, object?>> indexExpression) => HasIndex(indexExpression);
#endif

		//
		// Summary:
		//     Configures an index on the specified properties with the given name. If there
		//     is an existing index on the given list of properties and with the given name,
		//     then the existing index will be returned for configuration.
		//
		// Parameters:
		//   indexExpression:
		//     A lambda expression representing the property(s) to be included in the index
		//     (blog => blog.Url).
		//
		//     If the index is made up of multiple properties then specify an anonymous type
		//     including the properties (post => new { post.Title, post.BlogId }).
		//
		//   name:
		//     The name to assign to the index.
		//
		// Returns:
		//     An object that can be used to configure the index.
#if NetCore
		public virtual IndexBuilder<TEntity> HasIndex(Expression<Func<TEntity, object?>> indexExpression, string name) => Model.HasIndex(indexExpression, name);
#elif NetFX
		public virtual IndexConfiguration HasIndex(Expression<Func<TEntity, object?>> indexExpression, string name) => base.HasIndex(indexExpression).HasName(name);
#endif

		//
		// Summary:
		//     Configures an unnamed index on the specified properties. If there is an existing
		//     index on the given list of properties, then the existing index will be returned
		//     for configuration.
		//
		// Parameters:
		//   propertyNames:
		//     The names of the properties that make up the index.
		//
		// Returns:
		//     An object that can be used to configure the index.
#if NetCore
		public new virtual IndexBuilder<TEntity> HasIndex(params string[] propertyNames) => Model.HasIndex(propertyNames);
#endif

		//
		// Summary:
		//     Configures an index on the specified properties with the given name. If there
		//     is an existing index on the given list of properties and with the given name,
		//     then the existing index will be returned for configuration.
		//
		// Parameters:
		//   propertyNames:
		//     The names of the properties that make up the index.
		//
		//   name:
		//     The name to assign to the index.
		//
		// Returns:
		//     An object that can be used to configure the index.
#if NetCore
		public new virtual IndexBuilder<TEntity> HasIndex(string[] propertyNames, string name) => Model.HasIndex(propertyNames, name);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   navigationName:
		//     The name of the reference navigation property on this entity type that represents
		//     the relationship.
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the owned type and the relationship.
		//
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual OwnedNavigationBuilder<TEntity, TRelatedEntity> OwnsOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(string navigationName) where TRelatedEntity : class => Model.OwnsOne<TRelatedEntity>(navigationName);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   ownedTypeName:
		//     The name of the entity type that this relationship targets.
		//
		//   navigationName:
		//     The name of the reference navigation property on this entity type that represents
		//     the relationship.
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the owned type and the relationship.
		//
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual OwnedNavigationBuilder<TEntity, TRelatedEntity> OwnsOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(string ownedTypeName, string navigationName) where TRelatedEntity : class => Model.OwnsOne<TRelatedEntity>(ownedTypeName, navigationName);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   navigationExpression:
		//     A lambda expression representing the reference navigation property on this entity
		//     type that represents the relationship (customer => customer.Address).
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the owned type and the relationship.
		//
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual OwnedNavigationBuilder<TEntity, TRelatedEntity> OwnsOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(Expression<Func<TEntity, TRelatedEntity?>> navigationExpression) where TRelatedEntity : class => Model.OwnsOne(navigationExpression);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   ownedTypeName:
		//     The name of the entity type that this relationship targets.
		//
		//   navigationExpression:
		//     A lambda expression representing the reference navigation property on this entity
		//     type that represents the relationship (customer => customer.Address).
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the owned type and the relationship.
		//
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual OwnedNavigationBuilder<TEntity, TRelatedEntity> OwnsOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(string ownedTypeName, Expression<Func<TEntity, TRelatedEntity?>> navigationExpression) where TRelatedEntity : class => Model.OwnsOne(ownedTypeName, navigationExpression);

#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   navigationName:
		//     The name of the reference navigation property on this entity type that represents
		//     the relationship.
		//
		//   buildAction:
		//     An action that performs configuration of the owned type and the relationship.
		//
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the entity type.
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> OwnsOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(string navigationName, Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class => Model.OwnsOne(navigationName, buildAction);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   ownedTypeName:
		//     The name of the entity type that this relationship targets.
		//
		//   navigationName:
		//     The name of the reference navigation property on this entity type that represents
		//     the relationship.
		//
		//   buildAction:
		//     An action that performs configuration of the owned type and the relationship.
		//
		//
		// Returns:
		//     An object that can be used to configure the entity type.
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> OwnsOne(string ownedTypeName, string navigationName, Action<OwnedNavigationBuilder> buildAction) => Model.OwnsOne(ownedTypeName, navigationName, buildAction);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   ownedType:
		//     The entity type that this relationship targets.
		//
		//   navigationName:
		//     The name of the reference navigation property on this entity type that represents
		//     the relationship.
		//
		//   buildAction:
		//     An action that performs configuration of the owned type and the relationship.
		//
		//
		// Returns:
		//     An object that can be used to configure the entity type.
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> OwnsOne([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type ownedType, string navigationName, Action<OwnedNavigationBuilder> buildAction) => Model.OwnsOne(ownedType, navigationName, buildAction);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   ownedTypeName:
		//     The name of the entity type that this relationship targets.
		//
		//   ownedType:
		//     The CLR type of the entity type that this relationship targets.
		//
		//   navigationName:
		//     The name of the reference navigation property on this entity type that represents
		//     the relationship.
		//
		//   buildAction:
		//     An action that performs configuration of the owned type and the relationship.
		//
		//
		// Returns:
		//     An object that can be used to configure the entity type.
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public new virtual EntityTypeBuilder<TEntity> OwnsOne(string ownedTypeName, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type ownedType, string navigationName, Action<OwnedNavigationBuilder> buildAction) => Model.OwnsOne(ownedTypeName, ownedType, navigationName, buildAction);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   ownedTypeName:
		//     The name of the entity type that this relationship targets.
		//
		//   navigationName:
		//     The name of the reference navigation property on this entity type that represents
		//     the relationship.
		//
		//   buildAction:
		//     An action that performs configuration of the owned type and the relationship.
		//
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the entity type.
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> OwnsOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(string ownedTypeName, string navigationName, Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class => Model.OwnsOne(ownedTypeName, navigationName, buildAction);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   navigationExpression:
		//     A lambda expression representing the reference navigation property on this entity
		//     type that represents the relationship (customer => customer.Address).
		//
		//   buildAction:
		//     An action that performs configuration of the owned type and the relationship.
		//
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the entity type.
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> OwnsOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(Expression<Func<TEntity, TRelatedEntity?>> navigationExpression, Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class => Model.OwnsOne(navigationExpression, buildAction);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   ownedTypeName:
		//     The name of the entity type that this relationship targets.
		//
		//   navigationExpression:
		//     A lambda expression representing the reference navigation property on this entity
		//     type that represents the relationship (customer => customer.Address).
		//
		//   buildAction:
		//     An action that performs configuration of the owned type and the relationship.
		//
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the entity type.
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> OwnsOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(string ownedTypeName, Expression<Func<TEntity, TRelatedEntity?>> navigationExpression, Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class => Model.OwnsOne(ownedTypeName, navigationExpression, buildAction);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   navigationName:
		//     The name of the reference navigation property on this entity type that represents
		//     the relationship.
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the owned type and the relationship.
		//
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual OwnedNavigationBuilder<TEntity, TRelatedEntity> OwnsMany<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(string navigationName) where TRelatedEntity : class => Model.OwnsMany<TRelatedEntity>(navigationName);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   ownedTypeName:
		//     The name of the entity type that this relationship targets.
		//
		//   navigationName:
		//     The name of the reference navigation property on this entity type that represents
		//     the relationship.
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the owned type and the relationship.
		//
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual OwnedNavigationBuilder<TEntity, TRelatedEntity> OwnsMany<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(string ownedTypeName, string navigationName) where TRelatedEntity : class => Model.OwnsMany<TRelatedEntity>(ownedTypeName, navigationName);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   navigationExpression:
		//     A lambda expression representing the reference navigation property on this entity
		//     type that represents the relationship (customer => customer.Address).
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the owned type and the relationship.
		//
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual OwnedNavigationBuilder<TEntity, TRelatedEntity> OwnsMany<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(Expression<Func<TEntity, IEnumerable<TRelatedEntity>?>> navigationExpression) where TRelatedEntity : class => Model.OwnsMany(navigationExpression);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   ownedTypeName:
		//     The name of the entity type that this relationship targets.
		//
		//   navigationExpression:
		//     A lambda expression representing the reference navigation property on this entity
		//     type that represents the relationship (customer => customer.Address).
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the owned type and the relationship.
		//
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual OwnedNavigationBuilder<TEntity, TRelatedEntity> OwnsMany<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(string ownedTypeName, Expression<Func<TEntity, IEnumerable<TRelatedEntity>?>> navigationExpression) where TRelatedEntity : class => OwnsMany(ownedTypeName, navigationExpression);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   navigationName:
		//     The name of the reference navigation property on this entity type that represents
		//     the relationship.
		//
		//   buildAction:
		//     An action that performs configuration of the owned type and the relationship.
		//
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the entity type.
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> OwnsMany<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(string navigationName, Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class => Model.OwnsMany(navigationName, buildAction);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   ownedTypeName:
		//     The name of the entity type that this relationship targets.
		//
		//   navigationName:
		//     The name of the reference navigation property on this entity type that represents
		//     the relationship.
		//
		//   buildAction:
		//     An action that performs configuration of the owned type and the relationship.
		//
		//
		// Returns:
		//     An object that can be used to configure the entity type.
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public new virtual EntityTypeBuilder<TEntity> OwnsMany(string ownedTypeName, string navigationName, Action<OwnedNavigationBuilder> buildAction) => Model.OwnsMany(ownedTypeName, navigationName, buildAction);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   ownedType:
		//     The entity type that this relationship targets.
		//
		//   navigationName:
		//     The name of the reference navigation property on this entity type that represents
		//     the relationship.
		//
		//   buildAction:
		//     An action that performs configuration of the owned type and the relationship.
		//
		//
		// Returns:
		//     An object that can be used to configure the entity type.
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public new virtual EntityTypeBuilder<TEntity> OwnsMany([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type ownedType, string navigationName, Action<OwnedNavigationBuilder> buildAction) => Model.OwnsMany(ownedType, navigationName, buildAction);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   ownedTypeName:
		//     The name of the entity type that this relationship targets.
		//
		//   ownedType:
		//     The CLR type of the entity type that this relationship targets.
		//
		//   navigationName:
		//     The name of the reference navigation property on this entity type that represents
		//     the relationship.
		//
		//   buildAction:
		//     An action that performs configuration of the owned type and the relationship.
		//
		//
		// Returns:
		//     An object that can be used to configure the entity type.
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public new virtual EntityTypeBuilder<TEntity> OwnsMany(string ownedTypeName, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type ownedType, string navigationName, Action<OwnedNavigationBuilder> buildAction) => Model.OwnsMany(ownedType, navigationName, buildAction);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   ownedTypeName:
		//     The name of the entity type that this relationship targets.
		//
		//   navigationName:
		//     The name of the reference navigation property on this entity type that represents
		//     the relationship.
		//
		//   buildAction:
		//     An action that performs configuration of the owned type and the relationship.
		//
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the entity type.
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> OwnsMany<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(string ownedTypeName, string navigationName, Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class => Model.OwnsMany(ownedTypeName, navigationName, buildAction);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   navigationExpression:
		//     A lambda expression representing the reference navigation property on this entity
		//     type that represents the relationship (customer => customer.Address).
		//
		//   buildAction:
		//     An action that performs configuration of the owned type and the relationship.
		//
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the entity type.
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> OwnsMany<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(Expression<Func<TEntity, IEnumerable<TRelatedEntity>?>> navigationExpression, Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class => Model.OwnsMany(navigationExpression, buildAction);
#endif

		//
		// Summary:
		//     Configures a relationship where the target entity is owned by (or part of) this
		//     entity.
		//
		// Parameters:
		//   ownedTypeName:
		//     The name of the entity type that this relationship targets.
		//
		//   navigationExpression:
		//     A lambda expression representing the reference navigation property on this entity
		//     type that represents the relationship (customer => customer.Address).
		//
		//   buildAction:
		//     An action that performs configuration of the owned type and the relationship.
		//
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the entity type.
		//
		// Remarks:
		//     The target entity type for each ownership relationship is treated as a different
		//     entity type even if the navigation is of the same type. Configuration of the
		//     target entity type isn't applied to the target entity type of other ownership
		//     relationships.
		//
		//     Most operations on an owned entity require accessing it through the owner entity
		//     using the corresponding navigation.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder`2.WithOwner(System.String)
		//     to fully configure the relationship.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> OwnsMany<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(string ownedTypeName, Expression<Func<TEntity, IEnumerable<TRelatedEntity>?>> navigationExpression, Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class => Model.OwnsMany(ownedTypeName, navigationExpression, buildAction);
#endif

		//
		// Summary:
		//     Configures a relationship where this entity type has a reference that points
		//     to a single instance of the other type in the relationship.
		//
		// Parameters:
		//   navigationName:
		//     The name of the reference navigation property on this entity type that represents
		//     the relationship. If no property is specified, the relationship will be configured
		//     without a navigation property on this end.
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the relationship.
		//
		// Remarks:
		//     Note that calling this method with no parameters will explicitly configure this
		//     side of the relationship to use no navigation property, even if such a property
		//     exists on the entity type. If the navigation property is to be used, then it
		//     must be specified.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.ReferenceNavigationBuilder`2.WithMany(System.String)
		//     or Microsoft.EntityFrameworkCore.Metadata.Builders.ReferenceNavigationBuilder`2.WithOne(System.String)
		//     to fully configure the relationship. Calling just this method without the chained
		//     call will not produce a valid relationship.
#if NetCore
		public virtual ReferenceNavigationBuilder<TEntity, TRelatedEntity> HasOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(string? navigationName) where TRelatedEntity : class => Model.HasOne<TRelatedEntity>(navigationName);
#endif

		//
		// Summary:
		//     Configures a relationship where this entity type has a reference that points
		//     to a single instance of the other type in the relationship.
		//
		// Parameters:
		//   navigationExpression:
		//     A lambda expression representing the reference navigation property on this entity
		//     type that represents the relationship (post => post.Blog). If no property is
		//     specified, the relationship will be configured without a navigation property
		//     on this end.
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the relationship.
		//
		// Remarks:
		//     Note that calling this method with no parameters will explicitly configure this
		//     side of the relationship to use no navigation property, even if such a property
		//     exists on the entity type. If the navigation property is to be used, then it
		//     must be specified.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.ReferenceNavigationBuilder`2.WithMany(System.Linq.Expressions.Expression{System.Func{`1,System.Collections.Generic.IEnumerable{`0}}})
		//     or Microsoft.EntityFrameworkCore.Metadata.Builders.ReferenceNavigationBuilder`2.WithOne(System.Linq.Expressions.Expression{System.Func{`1,`0}})
		//     to fully configure the relationship. Calling just this method without the chained
		//     call will not produce a valid relationship.
#if NetCore
		public virtual ReferenceNavigationBuilder<TEntity, TRelatedEntity> HasOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(Expression<Func<TEntity, TRelatedEntity?>>? navigationExpression = null) where TRelatedEntity : class => Model.HasOne(navigationExpression);
#endif

		//
		// Summary:
		//     Configures a relationship where this entity type has a collection that contains
		//     instances of the other type in the relationship.
		//
		// Parameters:
		//   navigationName:
		//     The name of the collection navigation property on this entity type that represents
		//     the relationship. If no property is specified, the relationship will be configured
		//     without a navigation property on this end.
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the relationship.
		//
		// Remarks:
		//     Note that calling this method with no parameters will explicitly configure this
		//     side of the relationship to use no navigation property, even if such a property
		//     exists on the entity type. If the navigation property is to be used, then it
		//     must be specified.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.CollectionNavigationBuilder`2.WithOne(System.Linq.Expressions.Expression{System.Func{`1,`0}})
		//     to fully configure the relationship. Calling just this method without the chained
		//     call will not produce a valid relationship.
#if NetCore
		public virtual CollectionNavigationBuilder<TEntity, TRelatedEntity> HasMany<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(string? navigationName) where TRelatedEntity : class => Model.HasMany<TRelatedEntity>(navigationName);
#endif

		//
		// Summary:
		//     Configures a relationship where this entity type has a collection that contains
		//     instances of the other type in the relationship.
		//
		// Parameters:
		//   navigationExpression:
		//     A lambda expression representing the collection navigation property on this entity
		//     type that represents the relationship (blog => blog.Posts). If no property is
		//     specified, the relationship will be configured without a navigation property
		//     on this end.
		//
		// Type parameters:
		//   TRelatedEntity:
		//     The entity type that this relationship targets.
		//
		// Returns:
		//     An object that can be used to configure the relationship.
		//
		// Remarks:
		//     Note that calling this method with no parameters will explicitly configure this
		//     side of the relationship to use no navigation property, even if such a property
		//     exists on the entity type. If the navigation property is to be used, then it
		//     must be specified.
		//
		//     After calling this method, you should chain a call to Microsoft.EntityFrameworkCore.Metadata.Builders.CollectionNavigationBuilder`2.WithOne(System.Linq.Expressions.Expression{System.Func{`1,`0}})
		//     to fully configure the relationship. Calling just this method without the chained
		//     call will not produce a valid relationship.
#if NetCore
		public virtual CollectionNavigationBuilder<TEntity, TRelatedEntity> HasMany<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TRelatedEntity>(Expression<Func<TEntity, IEnumerable<TRelatedEntity>?>>? navigationExpression = null) where TRelatedEntity : class => Model.HasMany(navigationExpression);
#endif

		//
		// Summary:
		//     Configures the Microsoft.EntityFrameworkCore.ChangeTrackingStrategy to be used
		//     for this entity type. This strategy indicates how the context detects changes
		//     to properties for an instance of the entity type.
		//
		// Parameters:
		//   changeTrackingStrategy:
		//     The change tracking strategy to be used.
		//
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> HasChangeTrackingStrategy(ChangeTrackingStrategy changeTrackingStrategy) => Model.HasChangeTrackingStrategy(changeTrackingStrategy);
#endif

		//
		// Summary:
		//     Sets the Microsoft.EntityFrameworkCore.PropertyAccessMode to use for all properties
		//     of this entity type.
		//
		// Parameters:
		//   propertyAccessMode:
		//     The Microsoft.EntityFrameworkCore.PropertyAccessMode to use for properties of
		//     this entity type.
		//
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
		//
		//
		// Remarks:
		//     By default, the backing field, if one is found by convention or has been specified,
		//     is used when new objects are constructed, typically when entities are queried
		//     from the database. Properties are used for all other accesses. Calling this method
		//     will change that behavior for all properties of this entity type as described
		//     in the Microsoft.EntityFrameworkCore.PropertyAccessMode enum.
		//
		//     Calling this method overrides for all properties of this entity type any access
		//     mode that was set on the model.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> UsePropertyAccessMode(PropertyAccessMode propertyAccessMode) => Model.UsePropertyAccessMode(propertyAccessMode);

#endif

#if NetCore
		class EntityData : IEnumerable<TEntity>
		{
			Func<IEnumerable<TEntity>> dataProvider;
			IEnumerable<TEntity> data = null;
			public EntityData(Func<IEnumerable<TEntity>> dataProvider) { this.dataProvider = dataProvider; }
			public IEnumerator<TEntity> GetEnumerator()
			{
				if (data == null) data = dataProvider();
				if (!(data is Array || data is ICollection)) data = data.ToArray();

				foreach (var d in data) yield return d;
			}
			IEnumerator IEnumerable.GetEnumerator()
			{
				if (data == null) data = dataProvider();
				if (!(data is Array || data is ICollection)) data = data.ToArray();

				foreach (var d in data) yield return d;
			}
		}
		static TEntity[] emptyData = new TEntity[0];
		public virtual DataBuilder<TEntity> HasData(Func<IEnumerable<TEntity>> dataProvider) => InitSeedData ? Model.HasData(new EntityData(dataProvider)) : Model.HasData(emptyData);
#endif
#if NetFX
		public virtual object HasData(Func<IEnumerable<TEntity>> dataProvider) => new object();
#endif



		//
		// Summary:
		//     Adds seed data to this entity type. It is used to generate data motion migrations.
		//
		//
		// Parameters:
		//   data:
		//     An array of seed data of the same type as the entity.
		//
		// Returns:
		//     An object that can be used to configure the model data.
#if NetCore
		//public virtual DataBuilder<TEntity> HasData(params TEntity[] data) => Model.HasData(data);
#elif NetFX
		//public virtual object HasData(params TEntity[] data) => new object();
#endif

		//
		// Summary:
		//     Adds seed data to this entity type. It is used to generate data motion migrations.
		//
		//
		// Parameters:
		//   data:
		//     A collection of seed data of the same type as the entity.
		//
		// Returns:
		//     An object that can be used to configure the model data.
#if NetCore
		//public virtual DataBuilder<TEntity> HasData(IEnumerable<TEntity> data) => HasData(data);
#elif NetFX
		//public virtual object HasData(IEnumerable<TEntity> data) => new object();
#endif

		//
		// Summary:
		//     Adds seed data to this entity type. It is used to generate data motion migrations.
		//
		//
		// Parameters:
		//   data:
		//     An array of seed data represented by anonymous types.
		//
		// Returns:
		//     An object that can be used to configure the model data.
#if NetCore
		//public virtual DataBuilder<TEntity> HasData(params object[] data) => Model.HasData(data);
#elif NetFX
		//public virtual object HasData(params object[] data) => new object();
#endif

		//
		// Summary:
		//     Adds seed data to this entity type. It is used to generate data motion migrations.
		//
		//
		// Parameters:
		//   data:
		//     A collection of seed data represented by anonymous types.
		//
		// Returns:
		//     An object that can be used to configure the model data.
#if NetCore
		//public virtual DataBuilder<TEntity> HasData(IEnumerable<object> data) => Model.HasData(data);
#elif NetFX
		//public virtual object HasData(IEnumerable<object> data) => new object();
#endif
	
		//
		// Summary:
		//     Configures the discriminator property used to identify the entity type in the
		//     store.
		//
		// Parameters:
		//   propertyExpression:
		//     A lambda expression representing the property to be used as the discriminator
		//     ( blog => blog.Discriminator).
		//
		// Type parameters:
		//   TDiscriminator:
		//     The type of values stored in the discriminator property.
		//
		// Returns:
		//     A builder that allows the discriminator property to be configured.
#if NetCore
		public virtual DiscriminatorBuilder<TDiscriminator> HasDiscriminator<TDiscriminator>(Expression<Func<TEntity, TDiscriminator>> propertyExpression) => Model.HasDiscriminator(propertyExpression);
#endif

		//
		// Summary:
		//     Configures the entity type as having no discriminator property.
		//
		// Returns:
		//     The same builder instance so that multiple configuration calls can be chained.
#if NetCore
		public virtual EntityTypeBuilder<TEntity> HasNoDiscriminator() => Model.HasNoDiscriminator();
#endif
	}
}
