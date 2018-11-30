// This file describes the API that the generator will produce
//
// Authors:
//   MonoMac community
//   Miguel de Icaza
//
// Copyright 2009, 2011, MonoMac community
// Copyright 2011, Xamarin, Inc.
//
using System;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreData
{
	[BaseType (typeof (NSPersistentStore))]
	// Objective-C exception thrown.  Name: NSInternalInconsistencyException Reason: NSMappedObjectStore must be initialized with initWithPersistentStoreCoordinator:configurationName:URL:options
	[DisableDefaultCtor]
	public interface NSAtomicStore {

		[Export ("initWithPersistentStoreCoordinator:configurationName:URL:options:")]
		IntPtr Constructor (NSPersistentStoreCoordinator coordinator, string configurationName, NSUrl url, [NullAllowed] NSDictionary options);

		[Export ("load:")]
		bool Load (out NSError error);

		[Export ("save:")]
		bool Save (out NSError error);

		[Export ("newCacheNodeForManagedObject:")]
		NSAtomicStoreCacheNode NewCacheNodeForManagedObject (NSManagedObject managedObject);

		[Export ("updateCacheNode:fromManagedObject:")]
		void UpdateCacheNode (NSAtomicStoreCacheNode node, NSManagedObject managedObject);

		[Export ("cacheNodes")]
		NSSet CacheNodes { get; }

		[Export ("addCacheNodes:")]
		void AddCacheNodes (NSSet cacheNodes);

		[Export ("willRemoveCacheNodes:")]
		void WillRemoveCacheNodes (NSSet cacheNodes);

		[Export ("cacheNodeForObjectID:")]
		NSAtomicStoreCacheNode CacheNodeForObjectID (NSManagedObjectID objectID);

		[Export ("objectIDForEntity:referenceObject:")]
		NSManagedObjectID ObjectIDForEntity (NSEntityDescription entity, NSObject data);

		[Export ("newReferenceObjectForManagedObject:")]
		NSAtomicStore NewReferenceObjectForManagedObject (NSManagedObject managedObject);

		[Export ("referenceObjectForObjectID:")]
		NSAtomicStore ReferenceObjectForObjectID (NSManagedObjectID objectID);

	}
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: NSAtomicStoreCacheNodes must be initialized using initWithObjectID:(NSManagedObjectID *)
	[DisableDefaultCtor]
	public interface NSAtomicStoreCacheNode {

		[Export ("initWithObjectID:")]
		IntPtr Constructor (NSManagedObjectID moid);

		[Export ("objectID")]
		NSManagedObjectID ObjectID { get; }

		[Export ("propertyCache")]
		NSDictionary PropertyCache { get; set; }

		[Export ("valueForKey:")]
		NSAtomicStoreCacheNode ValueForKey (string key);

		[Export ("setValue:forKey:")]
		void SetValue (NSObject value, string key);

	}
	[BaseType (typeof (NSPropertyDescription))]
	public interface NSAttributeDescription {

		[Export ("attributeType")]
		NSAttributeType AttributeType { get; set; }

		[Export ("attributeValueClassName")]
		string AttributeValueClassName { get; set; }

		[Export ("defaultValue")]
		NSObject DefaultValue { get; set; }

		[Export ("versionHash")]
		NSData VersionHash { get; }

		[Export ("valueTransformerName")]
		string ValueTransformerName { get; set; }

		[Since(5,0)]
		[Export ("allowsExternalBinaryDataStorage")]
		bool AllowsExternalBinaryDataStorage { get; set; }
	}
	[BaseType (typeof (NSObject))]
	public interface NSEntityDescription {

		[Static, Export ("entityForName:inManagedObjectContext:")]
		NSEntityDescription EntityForName (string entityName, NSManagedObjectContext context);

		[Static, Export ("insertNewObjectForEntityForName:inManagedObjectContext:")]
		NSObject InsertNewObjectForEntityForName (string entityName, NSManagedObjectContext context);

		[Export ("managedObjectModel")]
		NSManagedObjectModel ManagedObjectModel { get; }

		[Export ("managedObjectClassName")]
		string ManagedObjectClassName { get; set; }

		[Export ("name")]
		string Name { get; set; }

		[Export ("Abstract")]
		bool Abstract { [Bind("isAbstract")] get; set; }

		[Export ("subentitiesByName")]
		NSDictionary SubentitiesByName { get; }

		[Export ("subentities")]
		NSEntityDescription[] Subentities { get; set; }

		[Export ("superentity")]
		NSEntityDescription Superentity { get; }

		[Export ("propertiesByName")]
		NSDictionary PropertiesByName { get; }

		[Export ("properties")]
		NSPropertyDescription[] Properties { get; set; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; set; }

		[Export ("attributesByName")]
		NSDictionary AttributesByName { get; }

		[Export ("relationshipsByName")]
		NSDictionary RelationshipsByName { get; }

		[Export ("relationshipsWithDestinationEntity:")]
		NSRelationshipDescription[] RelationshipsWithDestinationEntity (NSEntityDescription entity);

		[Export ("isKindOfEntity:")]
		bool IsKindOfEntity (NSEntityDescription entity);

		[Export ("versionHash")]
		NSData VersionHash { get; }

		[Export ("versionHashModifier")]
		string VersionHashModifier { get; set; }

		[Since(5,0)]
		[Export ("compoundIndexes")]
		NSPropertyDescription [] CompoundIndexes { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSEntityMapping {

		[Export ("name")]
		string Name { get; set; }

		[Export ("mappingType")]
		NSEntityMappingType MappingType { get; set; }

		[Export ("sourceEntityName")]
		string SourceEntityName { get; set; }

		[Export ("sourceEntityVersionHash")]
		NSData SourceEntityVersionHash { get; set; }

		[Export ("destinationEntityName")]
		string DestinationEntityName { get; set; }

		[Export ("destinationEntityVersionHash")]
		NSData DestinationEntityVersionHash { get; set; }

		[Export ("attributeMappings")]
		NSPropertyMapping[] AttributeMappings { get; set; }

		[Export ("relationshipMappings")]
		NSPropertyMapping[] RelationshipMappings { get; set; }

		[Export ("sourceExpression")]
		NSExpression SourceExpression { get; set; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; set; }

		[Export ("entityMigrationPolicyClassName")]
		string EntityMigrationPolicyClassName { get; set; }

	}
	[BaseType (typeof (NSObject))]
	public interface NSEntityMigrationPolicy {

		[Export ("beginEntityMapping:manager:error:")]
		bool BeginEntityMapping (NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		[Export ("createDestinationInstancesForSourceInstance:entityMapping:manager:error:")]
		bool CreateDestinationInstancesForSourceInstance (NSManagedObject sInstance, NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		[Export ("endInstanceCreationForEntityMapping:manager:error:")]
		bool EndInstanceCreationForEntityMapping (NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		[Export ("createRelationshipsForDestinationInstance:entityMapping:manager:error:")]
		bool CreateRelationshipsForDestinationInstance (NSManagedObject dInstance, NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		[Export ("endRelationshipCreationForEntityMapping:manager:error:")]
		bool EndRelationshipCreationForEntityMapping (NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		[Export ("performCustomValidationForEntityMapping:manager:error:")]
		bool PerformCustomValidationForEntityMapping (NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		[Export ("endEntityMapping:manager:error:")]
		bool EndEntityMapping (NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

	}
	[BaseType (typeof (NSPropertyDescription))]
	public interface NSFetchedPropertyDescription {

		[Export ("fetchRequest")]
		NSFetchRequest FetchRequest { get; set; }

	}

	[BaseType (typeof (NSPersistentStoreRequest))]
	public interface NSFetchRequest {

		[Export ("entity")]
		NSEntityDescription Entity { get; set; }

		[Export ("predicate")]
		NSPredicate Predicate { get; set; }

		[Export ("sortDescriptors")]
		NSSortDescriptor[] SortDescriptors { get; set; }

		[Export ("fetchLimit")]
		uint FetchLimit { get; set; }

		[Export ("affectedStores")]
		NSPersistentStore[] AffectedStores { get; set; }

		[Export ("resultType")]
		NSFetchRequestResultType ResultType { get; set; }

		[Export ("includesSubentities")]
		bool IncludesSubentities { get; set; }

		[Export ("includesPropertyValues")]
		bool IncludesPropertyValues { get; set; }

		[Export ("returnsObjectsAsFaults")]
		bool ReturnsObjectsAsFaults { get; set; }

		[Export ("relationshipKeyPathsForPrefetching")]
		string[] RelationshipKeyPathsForPrefetching { get; set; }

		[Since(5,0)]
		[Static]
		[Export ("fetchRequestWithEntityName:")]
		NSFetchRequest FromEntityName (string entityName);

		[Since(5,0)]
		[Export ("initWithEntityName:")]
		IntPtr Constructor (string entityName);

		[Since(5,0)]
		[Export ("entityName")]
		string EntityName { get; }

		[Since(5,0)]
		[Export ("fetchBatchSize")]
		int FetchBatchSize { get; set; }

		[Since(5,0)]
		[Export ("shouldRefreshRefetchedObjects")]
		bool ShouldRefreshRefetchedObjects { get; set; }

		[Since(5,0)]
		[Export ("havingPredicate")]
		NSPredicate HavingPredicate { get; set; }

		[Since(5,0)]
		[Export ("propertiesToGroupBy")]
		NSPropertyDescription [] PropertiesToGroupBy { get; set; }
	}
#if !MONOMAC
	[BaseType (typeof (NSObject), Delegates = new string [] { "WeakDelegate" })]
	interface NSFetchedResultsController {

		[Export ("initWithFetchRequest:managedObjectContext:sectionNameKeyPath:cacheName:")]
		IntPtr Constructor (NSFetchRequest fetchRequest, NSManagedObjectContext context, [NullAllowed] string sectionNameKeyPath, [NullAllowed] string name);

		[Wrap ("WeakDelegate")]
		NSFetchedResultsControllerDelegate Delegate { get; set; }

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Export ("cacheName")]
		string CacheName { get; }

		[Export ("fetchedObjects")]
		NSObject[] FetchedObjects { get; }

		[Export ("fetchRequest")]
		NSFetchRequest FetchRequest { get; }

		[Export ("managedObjectContext")]
		NSManagedObjectContext ManagedObjectContext { get; }

		[Export ("sectionNameKeyPath")]
		string SectionNameKeyPath { get; }

		[Export ("sections")]
		NSFetchedResultsSectionInfo[] Sections { get; }

		[Export ("performFetch:")]
		bool PerformFetch (out NSError error);

		[Export ("indexPathForObject:")]
		NSIndexPath FromObject (NSObject obj);

		[Export ("objectAtIndexPath:")]
		NSObject ObjectAt (NSIndexPath path);

		[Export ("sectionForSectionIndexTitle:atIndex:")]
		// name like UITableViewSource's similar (and linked) selector
		int SectionFor (string title, int atIndex);

		[Export ("sectionIndexTitleForSectionName:")]
		// again named like UITableViewSource
		string SectionIndexTitles (string sectionName);

		[Static]
		[Export ("deleteCacheWithName:")]
		void DeleteCache ([NullAllowed] string name);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface NSFetchedResultsControllerDelegate {
		[Export ("controllerWillChangeContent:")]
		void WillChangeContent (NSFetchedResultsController controller);

		[Export ("controller:didChangeObject:atIndexPath:forChangeType:newIndexPath:")]
		void DidChangeObject (NSFetchedResultsController controller, NSObject anObject, NSIndexPath indexPath, NSFetchedResultsChangeType type, NSIndexPath newIndexPath);

		[Export ("controller:didChangeSection:atIndex:forChangeType:")]
		void DidChangeSection (NSFetchedResultsController controller, NSFetchedResultsSectionInfo sectionInfo, uint sectionIndex, NSFetchedResultsChangeType type);

		[Export ("controllerDidChangeContent:")]
		void DidChangeContent (NSFetchedResultsController controller);

		[Export ("controller:sectionIndexTitleForSectionName:")]
		string SectionFor (NSFetchedResultsController controller, string sectionName);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface NSFetchedResultsSectionInfo {
		[Export ("numberOfObjects")]
		int Count { get; }

		[Export ("objects")]
		NSObject[] Objects { get; }

		[Export ("name")]
		string Name { get; }

		[Export ("indexTitle")]
		string IndexTitle { get; }
	}
#endif
	[Since(5,0)]
	[BaseType (typeof (NSPersistentStore))]
	interface NSIncrementalStore {
		[Export ("loadMetadata:")]
		bool LoadMetadata (out NSError error);

		[Export ("executeRequest:withContext:error:")]
		NSObject ExecuteRequest (NSPersistentStoreRequest request, NSManagedObjectContext context, out NSError error);

		[Export ("newValuesForObjectWithID:withContext:error:")]
		NSIncrementalStoreNode NewValues (NSManagedObjectID forObjectId, NSManagedObjectContext context, out NSError error);

		[Export ("newValueForRelationship:forObjectWithID:withContext:error:")]
		NSObject NewValue (NSRelationshipDescription forRelationship, NSManagedObjectID forObjectI, NSManagedObjectContext context, out NSError error);

		[Static]
		[Export ("identifierForNewStoreAtURL:")]
		NSObject IdentifierForNewStoreAtURL (NSUrl storeURL);

		[Export ("obtainPermanentIDsForObjects:error:")]
		NSObject [] ObtainPermanentIds (NSObject [] array, out NSError error);

		[Export ("managedObjectContextDidRegisterObjectsWithIDs:")]
		void ManagedObjectContextDidRegisterObjectsWithIds (NSObject [] objectIds);

		[Export ("managedObjectContextDidUnregisterObjectsWithIDs:")]
		void ManagedObjectContextDidUnregisterObjectsWithIds (NSObject [] objectIds);

		[Export ("newObjectIDForEntity:referenceObject:")]
		NSManagedObjectID NewObjectIdFor (NSEntityDescription forEntity, NSObject referenceObject);

		[Export ("referenceObjectForObjectID:")]
		NSObject ReferenceObjectForObject (NSManagedObjectID objectId);

	}

	[Since(5,0)]
	[BaseType (typeof (NSObject))]
	interface NSIncrementalStoreNode {
		[Export ("initWithObjectID:withValues:version:")]
		IntPtr Constructor (NSManagedObjectID objectId, NSDictionary values, ulong version);

		[Export ("updateWithValues:version:")]
		void Update (NSDictionary values, ulong version);

		[Export ("objectID")]
		NSManagedObjectID ObjectId { get; }

		[Export ("version")]
		long Version { get; }

		[Export ("valueForPropertyDescription:")]
		NSObject ValueForPropertyDescription (NSPropertyDescription prop);
	}

	[BaseType (typeof (NSObject))]
	// 'init' issues a warning: CoreData: error: Failed to call designated initializer on NSManagedObject class 'NSManagedObject' 
	// then crash while disposing the instance
	[DisableDefaultCtor]
	public interface NSManagedObject {
		[Export ("initWithEntity:insertIntoManagedObjectContext:")]
		IntPtr Constructor (NSEntityDescription entity, NSManagedObjectContext context);

		[Export ("managedObjectContext")]
		NSManagedObjectContext ManagedObjectContext { get; }

		[Export ("entity")]
		NSEntityDescription Entity { get; }

		[Export ("objectID")]
		NSManagedObjectID ObjectID { get; }

		[Export ("isInserted")]
		bool IsInserted { get; }

		[Export ("isUpdated")]
		bool IsUpdated { get; }

		[Export ("isDeleted")]
		bool IsDeleted { get; }

		[Export ("isFault")]
		bool IsFault { get; }

		[Export ("hasFaultForRelationshipNamed:")]
		bool HasFaultForRelationshipNamed (string key);

		[Export ("willAccessValueForKey:")]
		void WillAccessValueForKey (string key);

		[Export ("didAccessValueForKey:")]
		void DidAccessValueForKey (string key);

		[Export ("willChangeValueForKey:")]
		void WillChangeValueForKey (string key);

		[Export ("didChangeValueForKey:")]
		void DidChangeValueForKey (string key);

		[Export ("willChangeValueForKey:withSetMutation:usingObjects:")]
		void WillChangeValueForKey (string inKey, NSKeyValueSetMutationKind inMutationKind, NSSet inObjects);

		[Export ("didChangeValueForKey:withSetMutation:usingObjects:")]
		void DidChangeValueForKey (string inKey, NSKeyValueSetMutationKind inMutationKind, NSSet inObjects);

		[Export ("observationInfo")]
		IntPtr ObservationInfo { get; set; }

		[Export ("awakeFromFetch")]
		void AwakeFromFetch ();

		[Export ("awakeFromInsert")]
		void AwakeFromInsert ();

		[Export ("willSave")]
		void WillSave ();

		[Export ("didSave")]
		void DidSave ();

		[Export ("willTurnIntoFault")]
		void WillTurnIntoFault ();

		[Export ("didTurnIntoFault")]
		void DidTurnIntoFault ();

		[Export ("valueForKey:")]
		IntPtr ValueForKey (string key);

		[Export ("setValue:forKey:")]
		void SetValue (IntPtr value, string key);

		[Export ("primitiveValueForKey:")]
		IntPtr PrimitiveValueForKey (string key);

		[Export ("setPrimitiveValue:forKey:")]
		void SetPrimitiveValue (IntPtr value, string key);

		[Export ("committedValuesForKeys:")]
		NSDictionary CommittedValuesForKeys (string[] keys);

		[Export ("changedValues")]
		NSDictionary ChangedValues { get; }

		[Export ("validateValue:forKey:error:")]
		bool ValidateValue (NSObject value, string key, out NSError error);

		[Export ("validateForDelete:")]
		bool ValidateForDelete (out NSError error);

		[Export ("validateForInsert:")]
		bool ValidateForInsert (out NSError error);

		[Export ("validateForUpdate:")]
		bool ValidateForUpdate (out NSError error);

		[Since(5,0)]
		[Export ("hasChanges")]
		bool HasChanges { get; }

		[Export ("changedValuesForCurrentEvent")]
		NSDictionary ChangedValuesForCurrentEvent { get; }
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSManagedObjectContext {

		[Export ("persistentStoreCoordinator")]
		NSPersistentStoreCoordinator PersistentStoreCoordinator { get; set; }

		[Export ("undoManager")]
		NSUndoManager UndoManager { get; set; }

		[Export ("hasChanges")]
		bool HasChanges { get; }

		[Export ("objectRegisteredForID:")]
		NSManagedObject ObjectRegisteredForID (NSManagedObjectID objectID);

		[Export ("objectWithID:")]
		NSManagedObject ObjectWithID (NSManagedObjectID objectID);

		[Export ("executeFetchRequest:error:")]
		NSObject[] ExecuteFetchRequest (NSFetchRequest request, out NSError error);

		[Export ("countForFetchRequest:error:")]
		uint CountForFetchRequest (NSFetchRequest request, out NSError error);

		[Export ("insertObject:")]
		void InsertObject (NSManagedObject object1);

		[Export ("deleteObject:")]
		void DeleteObject (NSManagedObject object1);

		[Export ("refreshObject:mergeChanges:")]
		void RefreshObject (NSManagedObject object1, bool flag);

		[Export ("detectConflictsForObject:")]
		void DetectConflictsForObject (NSManagedObject object1);

		[Export ("observeValueForKeyPath:ofObject:change:context:")]
		void ObserveValueForKeyPath (string keyPath, IntPtr object1, NSDictionary change, IntPtr context);

		[Export ("processPendingChanges")]
		void ProcessPendingChanges ();

		[Export ("assignObject:toPersistentStore:")]
		void AssignObject (IntPtr object1, NSPersistentStore store);

		[Export ("insertedObjects")]
		NSSet InsertedObjects { get; }

		[Export ("updatedObjects")]
		NSSet UpdatedObjects { get; }

		[Export ("deletedObjects")]
		NSSet DeletedObjects { get; }

		[Export ("registeredObjects")]
		NSSet RegisteredObjects { get; }

		[Export ("undo")]
		void Undo ();

		[Export ("redo")]
		void Redo ();

		[Export ("reset")]
		void Reset ();

		[Export ("rollback")]
		void Rollback ();

		[Export ("save:")]
		bool Save (out NSError error);

		[Export ("lock")]
		void Lock ();

		[Export ("unlock")]
		void Unlock ();

		[Export ("tryLock")]
		bool TryLock { get; }

		[Export ("propagatesDeletesAtEndOfEvent")]
		bool PropagatesDeletesAtEndOfEvent { get; set; }

		[Export ("retainsRegisteredObjects")]
		bool RetainsRegisteredObjects { get; set; }

		[Export ("stalenessInterval")]
		double StalenessInterval { get; set; }

		[Export ("mergePolicy")]
		IntPtr MergePolicy { get; set; }

		[Export ("obtainPermanentIDsForObjects:error:")]
		bool ObtainPermanentIDsForObjects (NSManagedObject[] objects, out NSError error);

		[Export ("mergeChangesFromContextDidSaveNotification:")]
		void MergeChangesFromContextDidSaveNotification (NSNotification notification);

		// 5.0
		[Export ("initWithConcurrencyType:")]
		IntPtr Constructor (NSManagedObjectContextConcurrencyType ct);

		[Export ("performBlock:")]
		void Perform (/* non null */ Action action);

		[Export ("performBlockAndWait:")]
		void PerformAndWait (/* non null */ Action action);

		[Export ("userInfo")]
		NSMutableDictionary UserInfo { get; }

		[Export ("concurrencyType")]
		NSManagedObjectContextConcurrencyType ConcurrencyType { get; }

		//Detected properties
		[Export ("parentContext")]
		NSManagedObjectContext ParentContext { get; set; }
	}

	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** -URIRepresentation cannot be sent to an abstract object of class NSManagedObjectID: Create a concrete instance!
	[DisableDefaultCtor]
	public interface NSManagedObjectID {

		[Export ("entity")]
		NSEntityDescription Entity { get; }

		[Export ("persistentStore")]
		NSPersistentStore PersistentStore { get; }

		[Export ("isTemporaryID")]
		bool IsTemporaryID { get; }

		[Export ("URIRepresentation")]
		NSUrl URIRepresentation { get; }

	}
	[BaseType (typeof (NSObject))]
	public interface NSManagedObjectModel {

		[Static, Export ("mergedModelFromBundles:")]
		NSManagedObjectModel MergedModelFromBundles (NSBundle[] bundles);

		[Static, Export ("modelByMergingModels:")]
		NSManagedObjectModel ModelByMergingModels (NSManagedObjectModel[] models);

		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl url);

		[Export ("entitiesByName")]
		NSDictionary EntitiesByName { get; }

		[Export ("entities")]
		NSEntityDescription[] Entities { get; set; }

		[Export ("configurations")]
		string[] Configurations { get; }

		[Export ("entitiesForConfiguration:")]
		string[] EntitiesForConfiguration (string configuration);

		[Export ("setEntities:forConfiguration:")]
		void SetEntities (NSEntityDescription[] entities, string configuration);

		[Export ("setFetchRequestTemplate:forName:")]
		void SetFetchRequestTemplate (NSFetchRequest fetchRequestTemplate, string name);

		[Export ("fetchRequestTemplateForName:")]
		NSFetchRequest FetchRequestTemplateForName (string name);

		[Export ("fetchRequestFromTemplateWithName:substitutionVariables:")]
		NSFetchRequest FetchRequestFromTemplateWithName (string name, NSDictionary variables);

		[Export ("localizationDictionary")]
		NSDictionary LocalizationDictionary { get; set; }

		[Static, Export ("mergedModelFromBundles:forStoreMetadata:")]
		NSManagedObjectModel MergedModelFromBundles (NSBundle[] bundles, NSDictionary metadata);

		[Static, Export ("modelByMergingModels:forStoreMetadata:")]
		NSManagedObjectModel ModelByMergingModels (NSManagedObjectModel[] models, NSDictionary metadata);

		[Export ("fetchRequestTemplatesByName")]
		NSDictionary FetchRequestTemplatesByName { get; }

		[Export ("versionIdentifiers")]
		NSSet VersionIdentifiers { get; set; }

		[Export ("isConfiguration:compatibleWithStoreMetadata:")]
		bool IsConfiguration (string configuration, NSDictionary metadata);

		[Export ("entityVersionHashesByName")]
		NSDictionary EntityVersionHashesByName { get; }
	}
	[BaseType (typeof (NSObject))]
	public interface NSMappingModel {

		[Static, Export ("mappingModelFromBundles:forSourceModel:destinationModel:")]
		NSMappingModel MappingModelFromBundles (NSBundle[] bundles, NSManagedObjectModel sourceModel, NSManagedObjectModel destinationModel);

		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl url);

		[Export ("entityMappings")]
		NSEntityMapping[] EntityMappings { get; set; }

		[Export ("entityMappingsByName")]
		NSDictionary EntityMappingsByName { get; }

	}

	[Since(5,0)]
	[BaseType (typeof (NSObject))]
	interface NSMergeConflict {
		[Export ("sourceObject")]
		NSManagedObject SourceObject { get;  }

		[Export ("objectSnapshot")]
		NSDictionary ObjectSnapshot { get;  }

		[Export ("cachedSnapshot")]
		NSDictionary CachedSnapshot { get;  }

		[Export ("persistedSnapshot")]
		NSDictionary PersistedSnapshot { get;  }

		[Export ("newVersionNumber")]
		uint NewVersionNumber { get;  }

		[Export ("oldVersionNumber")]
		uint OldVersionNumber { get;  }

		[Export ("initWithSource:newVersion:oldVersion:cachedSnapshot:persistedSnapshot:")]
		IntPtr Constructor (NSManagedObject srcObject, uint newvers, uint oldvers, NSDictionary cachesnap, NSDictionary persnap);
	}

	[Since(5,0)]
	[BaseType (typeof (NSObject))]
	interface NSMergePolicy {
		[Export ("mergeType")]
		NSMergePolicyType MergeType { get;  }

		[Export ("initWithMergeType:")]
		IntPtr Constructor (NSMergePolicyType ty);

		[Export ("resolveConflicts:error:")]
		bool ResolveConflictserror (NSMergeConflict [] list, out NSError error);
	}

	[BaseType (typeof (NSObject))]
	public interface NSMigrationManager {

		[Export ("initWithSourceModel:destinationModel:")]
		IntPtr Constructor (NSManagedObjectModel sourceModel, NSManagedObjectModel destinationModel);

		[Export ("migrateStoreFromURL:type:options:withMappingModel:toDestinationURL:destinationType:destinationOptions:error:")]
		bool MigrateStoreFromUrl (NSUrl sourceURL, string sStoreType, NSDictionary sOptions, NSMappingModel mappings, NSUrl dURL, string dStoreType, NSDictionary dOptions, out NSError error);

		[Export ("reset")]
		void Reset ();

		[Export ("mappingModel")]
		NSMappingModel MappingModel { get; }

		[Export ("sourceModel")]
		NSManagedObjectModel SourceModel { get; }

		[Export ("destinationModel")]
		NSManagedObjectModel DestinationModel { get; }

		[Export ("sourceContext")]
		NSManagedObjectContext SourceContext { get; }

		[Export ("destinationContext")]
		NSManagedObjectContext DestinationContext { get; }

		[Export ("sourceEntityForEntityMapping:")]
		NSEntityDescription SourceEntityForEntityMapping (NSEntityMapping mEntity);

		[Export ("destinationEntityForEntityMapping:")]
		NSEntityDescription DestinationEntityForEntityMapping (NSEntityMapping mEntity);

		[Export ("associateSourceInstance:withDestinationInstance:forEntityMapping:")]
		void AssociateSourceInstance (NSManagedObject sourceInstance, NSManagedObject destinationInstance, NSEntityMapping entityMapping);

		[Export ("destinationInstancesForEntityMappingNamed:sourceInstances:")]
		NSManagedObject[] DestinationInstancesForEntityMappingNamed (string mappingName, NSManagedObject[] sourceInstances);

		[Export ("sourceInstancesForEntityMappingNamed:destinationInstances:")]
		NSManagedObject[] SourceInstancesForEntityMappingNamed (string mappingName, NSManagedObject[] destinationInstances);

		[Export ("currentEntityMapping")]
		NSEntityMapping CurrentEntityMapping { get; }

		[Export ("migrationProgress")]
		float MigrationProgress { get; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; set; }

		[Export ("cancelMigrationWithError:")]
		void CancelMigrationWithError (NSError error);

		// 5.0
		[Since(5,0)]
		[Export ("usesStoreSpecificMigrationManager")]
		bool UsesStoreSpecificMigrationManager { get; set; }
	}
	[BaseType (typeof (NSObject))]
	public interface NSPersistentStore {

		[Static, Export ("metadataForPersistentStoreWithURL:error:")]
		NSDictionary MetadataForPersistentStoreWithUrl (NSUrl url, out NSError error);

		[Static, Export ("setMetadata:forPersistentStoreWithURL:error:")]
		bool SetMetadata (NSDictionary metadata, NSUrl url, out NSError error);

		[Export ("initWithPersistentStoreCoordinator:configurationName:URL:options:")]
		IntPtr Constructor (NSPersistentStoreCoordinator root, string name, NSUrl url, NSDictionary options);

		[Export ("persistentStoreCoordinator")]
		NSPersistentStoreCoordinator PersistentStoreCoordinator { get; }

		[Export ("configurationName")]
		string ConfigurationName { get; }

		[Export ("options")]
		NSDictionary Options { get; }

		[Export ("URL")]
		NSUrl Url { get; set; }

		[Export ("identifier")]
		string Identifier { get; set; }

		[Export ("type")]
		string Type { get; }

		[Export ("isReadOnly")]
		bool ReadOnly { get; [Bind("setReadOnly:")] set; }

		[Export ("metadata")]
		NSDictionary Metadata { get; set; }

		[Export ("didAddToPersistentStoreCoordinator:")]
		void DidAddToPersistentStoreCoordinator (NSPersistentStoreCoordinator coordinator);

		[Export ("willRemoveFromPersistentStoreCoordinator:")]
		void WillRemoveFromPersistentStoreCoordinator (NSPersistentStoreCoordinator coordinator);

		[Field ("NSPersistentStoreSaveConflictsErrorKey")]
		NSString SaveConflictsErrorKey { get; }

	}
	[BaseType (typeof (NSObject))]
	public interface NSPersistentStoreCoordinator {

		[Static, Export ("registeredStoreTypes")]
		NSDictionary RegisteredStoreTypes { get; }

		[Static, Export ("registerStoreClass:forStoreType:")]
		void RegisterStoreClass (Class storeClass, NSString storeType);

		[Static, Export ("metadataForPersistentStoreOfType:URL:error:")]
		NSDictionary MetadataForPersistentStoreOfType (NSString storeType, NSUrl url, out NSError error);

		[Static, Export ("setMetadata:forPersistentStoreOfType:URL:error:")]
		bool SetMetadata (NSDictionary metadata, NSString storeType, NSUrl url, out NSError error);

		[Export ("setMetadata:forPersistentStore:")]
		void SetMetadata (NSDictionary metadata, NSPersistentStore store);

		[Export ("metadataForPersistentStore:")]
		NSDictionary MetadataForPersistentStore (NSPersistentStore store);

		[Export ("initWithManagedObjectModel:")]
		IntPtr Constructor (NSManagedObjectModel model);

		[Export ("managedObjectModel")]
		NSManagedObjectModel ManagedObjectModel { get; }

		[Export ("persistentStores")]
		NSPersistentStore[] PersistentStores { get; }

		[Export ("persistentStoreForURL:")]
		NSPersistentStore PersistentStoreForUrl (NSUrl URL);

		[Export ("URLForPersistentStore:")]
		NSUrl UrlForPersistentStore (NSPersistentStore store);

		[Export ("setURL:forPersistentStore:")]
		bool SetUrl (NSUrl url, NSPersistentStore store);

		[Export ("addPersistentStoreWithType:configuration:URL:options:error:")]
		NSPersistentStore AddPersistentStoreWithType (NSString storeType, [NullAllowed] string configuration, NSUrl storeURL, [NullAllowed] NSDictionary options, out NSError error);

		[Export ("removePersistentStore:error:")]
		bool RemovePersistentStore (NSPersistentStore store, out NSError error);

		[Export ("migratePersistentStore:toURL:options:withType:error:")]
		NSPersistentStore MigratePersistentStore (NSPersistentStore store, NSUrl URL, NSDictionary options, NSString storeType, out NSError error);

		[Export ("managedObjectIDForURIRepresentation:")]
		NSManagedObjectID ManagedObjectIDForURIRepresentation (NSUrl url);

		[Export ("lock")]
		void Lock ();

		[Export ("unlock")]
		void Unlock ();

		[Export ("tryLock")]
		bool TryLock { get; }

#if MONOMAC
		[Obsolete("Deprecated in MAC OSX 10.5 and later")]
		[Static, Export ("metadataForPersistentStoreWithURL:error:")]
		NSDictionary MetadataForPersistentStoreWithUrl (NSUrl url, out NSError error);
#endif
		[Field ("NSSQLiteStoreType")]
		NSString SQLiteStoreType { get; }
#if MONOMAC
		[Field ("NSXMLStoreType")]
		NSString XMLStoreType { get; }
#endif	
		[Field ("NSBinaryStoreType")]
		NSString BinaryStoreType { get; }
		
		[Field ("NSInMemoryStoreType")]
		NSString InMemoryStoreType { get; }

		[Field ("NSStoreUUIDKey")]
		NSString StoreUUIDKey { get; }

		[Field ("NSAddedPersistentStoresKey")]
		NSString AddedPersistentStoresKey { get; }

		[Field ("NSRemovedPersistentStoresKey")]
		NSString RemovedPersistentStoresKey { get; }

		[Field ("NSUUIDChangedPersistentStoresKey")]
		NSString UUIDChangedPersistentStoresKey { get; }

		[Field ("NSReadOnlyPersistentStoreOption")]
		NSString ReadOnlyPersistentStoreOption { get; }
#if MONOMAC
		[Field ("NSValidateXMLStoreOption")]
		NSString ValidateXMLStoreOption { get; }
#endif
		[Field ("NSPersistentStoreTimeoutOption")]
		NSString PersistentStoreTimeoutOption { get; }

		[Field ("NSSQLitePragmasOption")]
		NSString SQLitePragmasOption { get; }

		[Field ("NSSQLiteAnalyzeOption")]
		NSString SQLiteAnalyzeOption { get; }

		[Field ("NSSQLiteManualVacuumOption")]
		NSString SQLiteManualVacuumOption { get; }

		[Field ("NSIgnorePersistentStoreVersioningOption")]
		NSString IgnorePersistentStoreVersioningOption { get; }

		[Field ("NSMigratePersistentStoresAutomaticallyOption")]
		NSString MigratePersistentStoresAutomaticallyOption { get; }

		[Field ("NSInferMappingModelAutomaticallyOption")]
		NSString InferMappingModelAutomaticallyOption { get; }

		[Field ("NSStoreModelVersionHashesKey")]
		NSString StoreModelVersionHashesKey { get; }

		[Field ("NSStoreModelVersionIdentifiersKey")]
		NSString StoreModelVersionIdentifiersKey { get; }

		[Field ("NSPersistentStoreOSCompatibility")]
		NSString PersistentStoreOSCompatibility { get; }

		[Field ("NSStoreTypeKey")]
		NSString StoreTypeKey { get; }

		[Field ("NSPersistentStoreCoordinatorStoresDidChangeNotification")]
		NSString StoresDidChangeNotification { get; }

		[Field ("NSPersistentStoreCoordinatorWillRemoveStoreNotification")]
		NSString WillRemoveStoreNotification { get; }
		
		// 5.0
		[Since(5,0)]
		[Export ("executeRequest:withContext:error:")]
		NSObject ExecuteRequestwithContexterror (NSPersistentStoreRequest request, NSManagedObjectContext context, out NSError error);

		[Field ("NSPersistentStoreDidImportUbiquitousContentChangesNotification")]
		NSString DidImportUbiquitousContentChangesNotification { get; }

		
		[Field ("NSPersistentStoreUbiquitousContentNameKey")]
		NSString PersistentStoreUbiquitousContentNameKey { get; }

		[Field ("NSPersistentStoreUbiquitousContentURLKey")]
		NSString PersistentStoreUbiquitousContentUrlLKey { get; }
#if !MONOMAC
		[Field ("NSPersistentStoreFileProtectionKey")]
		NSString PersistentStoreFileProtectionKey { get; }
#endif
	}

	[BaseType (typeof (NSObject))]
	public interface NSPersistentStoreRequest {
		[Export ("requestType")]
		NSPersistentStoreRequestType RequestType { get; }

		//Detected properties
		[Export ("affectedStores")]
		NSPersistentStore [] AffectedStores { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSPropertyDescription {

		[Export ("entity")]
		NSEntityDescription Entity { get; }

		[Export ("name")]
		string Name { get; set; }

		[Export ("isOptional")]
		bool Optional { get; [Bind("setOptional:")] set; }

		[Export ("isTransient")]
		bool Transient { get; [Bind("setTransient:")] set; }

		[Export ("validationPredicates")]
		NSPredicate[] ValidationPredicates { get; }

		[Export ("validationWarnings")]
		string[] ValidationWarnings { get; }

		[Export ("setValidationPredicates:withValidationWarnings:")]
		void SetValidationPredicates (NSPredicate[] validationPredicates, string[] validationWarnings);

		[Export ("userInfo")]
		NSDictionary UserInfo { get; set; }

		[Export ("isIndexed")]
		bool Indexed { get; [Bind("setIndexed:")] set; }

		[Export ("versionHash")]
		NSData VersionHash { get; }

		[Export ("versionHashModifier")]
		string VersionHashModifier { get; set; }

		// 5.0
		[Since (5,0)]
		[Export ("indexedBySpotlight")]
		bool IndexedBySpotlight { [Bind ("isIndexedBySpotlight")]get; set; }

		[Since (5,0)]
		[Export ("storedInExternalRecord")]
		bool StoredInExternalRecord { [Bind ("isStoredInExternalRecord")]get; set; }
	}
	[BaseType (typeof (NSObject))]
	public interface NSPropertyMapping {

		[Export ("name")]
		string Name { get; set; }

		[Export ("valueExpression")]
		NSExpression ValueExpression { get; set; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; set; }

	}
	[BaseType (typeof (NSPropertyDescription))]
	public interface NSRelationshipDescription {

		[Export ("destinationEntity")]
		NSEntityDescription DestinationEntity { get; set; }

		[Export ("inverseRelationship")]
		NSRelationshipDescription InverseRelationship { get; set; }

		[Export ("maxCount")]
		uint MaxCount { get; set; }

		[Export ("minCount")]
		uint MinCount { get; set; }

		[Export ("deleteRule")]
		NSDeleteRule DeleteRule { get; set; }

		[Export ("isToMany")]
		bool IsToMany { get; }

		[Export ("versionHash")]
		NSData VersionHash { get; }

		// 5.0
		[Since (5,0)]
		[Export ("ordered")]
		bool Ordered { [Bind ("isOrdered")]get; set; }
	}

	[BaseType (typeof (NSPersistentStoreRequest))]
	interface NSSaveChangesRequest {
		[Export ("initWithInsertedObjects:updatedObjects:deletedObjects:lockedObjects:")]
		IntPtr Constructor (NSSet insertedObjects, NSSet updatedObjects, NSSet deletedObjects, NSSet lockedObjects);

		[Export ("insertedObjects")]
		NSSet InsertedObjects { get; }

		[Export ("updatedObjects")]
		NSSet UpdatedObjects { get; }

		[Export ("deletedObjects")]
		NSSet DeletedObjects { get; }

		[Export ("lockedObjects")]
		NSSet LockedObjects { get; }
	}

}

