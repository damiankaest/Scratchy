# MongoDB Migration Guide for Scratchy Backend Application

This guide provides step-by-step prompts for an AI agent to refactor the Scratchy application from a hybrid SQL/MongoDB setup to a pure MongoDB-only architecture with automatic schema creation on startup.

## Current State Analysis

The application currently uses:
- Entity Framework Core with SQL Server
- Some MongoDB integration
- Hybrid repository pattern
- SQL-based DbContext and migrations

## Target State

- **MongoDB-only** data persistence
- Automatic MongoDB schema/collection creation on startup
- MongoDB-native repositories
- No Entity Framework dependencies
- Clean MongoDB document models

---

## Phase 1: Analysis and Planning

### Prompt 1: Analyze Current Data Models
```
Examine the current Entity Framework models in the Scratchy.Domain project. List all entities currently defined in the ScratchItDbContext and their relationships. Create a mapping document that shows:
1. Current SQL entities and their properties
2. Entity relationships (one-to-many, many-to-many)
3. Primary keys and foreign keys
4. Required MongoDB document structure for each entity
5. How to handle relationships in MongoDB (embedded vs referenced documents)

Focus on these key areas:
- User management and authentication
- Music entities (Artist, Album, Track)
- Social features (Follow, Post, Comment)
- Playlists and Scratches
- Notifications and badges
```

### Prompt 2: Design MongoDB Schema Strategy
```
Based on the current entity analysis, design a MongoDB schema strategy that:
1. Converts SQL relationships to MongoDB document patterns
2. Decides between embedded documents vs document references
3. Defines collection names and document structures
4. Plans for efficient querying patterns
5. Considers data consistency and transaction needs

Create detailed MongoDB document schemas for each collection, including:
- Field names and types
- Nested/embedded document structures
- Index requirements
- Validation rules
```

## Phase 2: MongoDB Infrastructure Setup

### Prompt 3: Remove Entity Framework Dependencies
```
Remove all Entity Framework dependencies from the project:
1. Remove EntityFrameworkCore packages from all .csproj files
2. Remove SQL Server connection strings from appsettings.json
3. Delete the Migrations folder in Scratchy.Persistence
4. Remove ScratchItDbContext.cs and ScratchItDbContextFactory.cs
5. Update the Scratchy.Persistence.csproj to remove EF references but keep MongoDB.Driver
6. Remove any EF-related using statements throughout the codebase
7. Remove the ConfigureDatabase method from ServiceExtension.cs that sets up EF DbContext
```

### Prompt 4: Create MongoDB Configuration
```
Create a new MongoDB configuration system:
1. Add MongoDB connection settings to appsettings.json and appsettings.Development.json
2. Create a MongoDbSettings configuration class in Scratchy.Domain/Configuration/
3. Create a MongoDB context class (MongoDbContext) in Scratchy.Persistence/DB/
4. Implement connection string management and database initialization
5. Add MongoDB service registration in ServiceExtension.cs
6. Ensure the MongoDB context can create collections and indexes on startup

The MongoDB connection should support:
- Connection string configuration
- Database name configuration
- Connection timeout and retry policies
- Environment-specific settings (dev, staging, prod)
```

### Prompt 5: Create MongoDB Document Models
```
Create new MongoDB document models in Scratchy.Domain/Models/ that replace the EF entities:
1. Convert each SQL entity to a MongoDB document class
2. Use MongoDB attributes for field mapping and validation
3. Implement proper ObjectId handling for document IDs
4. Handle relationships using either embedded documents or references
5. Add MongoDB-specific attributes like [BsonElement], [BsonId], etc.
6. Create base document classes if needed for common fields (like timestamps)

Ensure document models support:
- Proper serialization/deserialization
- BSON field naming conventions
- Date/time handling
- Nullable reference types
```

## Phase 3: Repository Pattern Migration

### Prompt 6: Create MongoDB Base Repository
```
Create a new base MongoDB repository pattern:
1. Create an IMongoRepository<T> interface in Scratchy.Domain/Interfaces/Repositories/
2. Implement a base MongoRepository<T> class in Scratchy.Persistence/Repositories/
3. Include common CRUD operations (Create, Read, Update, Delete)
4. Add support for async operations and cancellation tokens
5. Implement proper error handling and logging
6. Add support for MongoDB-specific operations like aggregation pipelines
7. Include methods for bulk operations and transactions where needed

The base repository should provide:
- Generic CRUD operations
- Query building capabilities
- Aggregation pipeline support
- Transaction support
- Performance optimization features
```

### Prompt 7: Migrate Individual Repositories
```
Migrate each existing repository to use MongoDB:
1. Update IUserRepository, IScratchRepository, IAlbumRepository, etc.
2. Implement MongoDB-specific query operations
3. Replace LINQ-to-SQL with MongoDB query builders or LINQ-to-MongoDB
4. Handle complex queries and joins using MongoDB aggregation pipelines
5. Migrate all existing repository methods to work with MongoDB documents
6. Add proper indexing strategies for query performance
7. Update repository registration in ServiceExtension.cs

For each repository, ensure:
- All existing functionality is preserved
- Query performance is optimized
- Proper error handling is implemented
- Unit tests are updated to work with MongoDB
```

## Phase 4: Service Layer Updates

### Prompt 8: Update Service Layer for MongoDB
```
Update all service classes in Scratchy.Application/Services/ to work with the new MongoDB repositories:
1. Update dependency injection to use the new MongoDB repositories
2. Modify service methods that relied on EF-specific features (like Include(), ThenInclude())
3. Update any LINQ queries that may not be compatible with MongoDB
4. Handle relationship loading differently (eager vs lazy loading)
5. Update transaction handling for MongoDB
6. Ensure all existing API contracts remain the same

Pay special attention to:
- Complex query operations
- Data aggregation and reporting
- Batch operations
- Search functionality
```

### Prompt 9: Update Controllers and DTOs
```
Review and update controllers and DTOs if needed:
1. Ensure all controller actions still work with the new data layer
2. Update any DTOs that might have EF-specific properties
3. Verify that all API endpoints return the same data structure
4. Update any controller logic that relied on EF change tracking
5. Test all CRUD operations through the API
6. Update PostResponseDto.cs and other response models if needed

Ensure backward compatibility:
- API contracts remain unchanged
- Response formats stay consistent
- Error handling maintains the same behavior
```

## Phase 5: Database Initialization and Schema Management

### Prompt 10: Implement Automatic Schema Creation
```
Create a MongoDB schema initialization system that runs on application startup:
1. Create a MongoDbInitializer class in Scratchy.Persistence/DB/
2. Implement automatic collection creation for all document types
3. Create indexes for optimal query performance
4. Add data seeding capabilities for initial/test data
5. Implement schema validation rules at the database level
6. Add database health check endpoints
7. Integrate the initializer into Program.cs startup sequence

The initializer should:
- Create all required collections
- Set up proper indexes
- Validate document schemas
- Handle database migration scenarios
- Provide detailed logging
```

### Prompt 11: Create Index Strategy
```
Implement a comprehensive indexing strategy for MongoDB:
1. Analyze query patterns from the existing repositories
2. Create indexes for frequently queried fields
3. Implement compound indexes for complex queries
4. Add text indexes for search functionality
5. Create unique indexes where data uniqueness is required
6. Set up TTL (Time To Live) indexes for temporary data
7. Monitor and optimize index performance

Index categories to implement:
- User lookup indexes (username, email, Firebase UID)
- Music catalog indexes (artist name, album title, track search)
- Social feature indexes (followers, posts, comments)
- Timestamp-based indexes for feeds and notifications
```

## Phase 6: Testing and Validation

### Prompt 12: Update Unit Tests
```
Update the unit test suite to work with MongoDB:
1. Replace InMemoryDbContextFactory with MongoDB test containers or in-memory MongoDB
2. Update repository tests to use MongoDB test data
3. Create test data builders for MongoDB documents
4. Update integration tests to use MongoDB
5. Add tests for MongoDB-specific functionality
6. Ensure all existing test cases pass with the new implementation
7. Add performance tests for critical queries

Test infrastructure needs:
- MongoDB test database setup
- Test data seeding
- Cleanup procedures
- Performance benchmarks
```

### Prompt 13: Integration Testing and Validation
```
Perform comprehensive integration testing:
1. Test all API endpoints with the new MongoDB backend
2. Verify data consistency and integrity
3. Test complex queries and aggregations
4. Validate performance under load
5. Test error scenarios and recovery
6. Verify that the Firebase authentication still works correctly
7. Test Azure Blob Storage integration remains functional
8. Validate that all existing functionality works as expected

Create validation checklist:
- User registration and authentication
- Music catalog operations
- Social features (following, posts, comments)
- Playlist management
- Notification system
- Search functionality
```

## Phase 7: Configuration and Deployment

### Prompt 14: Environment Configuration
```
Set up proper environment configuration for MongoDB:
1. Update appsettings.json files for all environments
2. Configure MongoDB connection strings for dev, staging, and production
3. Set up proper MongoDB Atlas or local MongoDB instances
4. Configure environment-specific database names
5. Set up connection pooling and timeout settings
6. Implement proper secrets management for connection strings
7. Update Azure application settings if deployed to Azure

Configuration requirements:
- Development: Local MongoDB or MongoDB Atlas free tier
- Staging: MongoDB Atlas shared cluster
- Production: MongoDB Atlas dedicated cluster with proper security
```

### Prompt 15: Performance Optimization
```
Optimize the MongoDB implementation for production:
1. Review and optimize all MongoDB queries
2. Implement proper connection pooling
3. Add query result caching where appropriate
4. Optimize document structures for common access patterns
5. Implement bulk operations for batch processing
6. Add monitoring and logging for database operations
7. Set up performance alerts and monitoring

Performance considerations:
- Query execution time monitoring
- Index usage analysis
- Memory usage optimization
- Connection pool monitoring
- Error rate tracking
```

## Phase 8: Documentation and Cleanup

### Prompt 16: Clean Up Legacy Code
```
Remove all remaining SQL/Entity Framework code:
1. Delete any unused migration files
2. Remove EF-related dependencies from package references
3. Clean up using statements that are no longer needed
4. Remove any commented-out EF code
5. Update documentation to reflect MongoDB usage
6. Remove SQL Server connection strings from configuration
7. Clean up any legacy repository implementations

Verification checklist:
- No Entity Framework packages remain
- No SQL Server connections
- All code compiles without EF references
- Application starts successfully
- All tests pass
```

### Prompt 17: Update Documentation
```
Create comprehensive documentation for the new MongoDB implementation:
1. Update README.md with MongoDB setup instructions
2. Document the new database schema and collections
3. Create API documentation with MongoDB examples
4. Document deployment procedures for different environments
5. Create troubleshooting guide for common MongoDB issues
6. Document backup and recovery procedures
7. Create performance tuning guide

Documentation should include:
- Local development setup with MongoDB
- Production deployment checklist
- Database administration tasks
- Monitoring and maintenance procedures
```

## Validation Checklist

After completing all phases, verify the following:

### Functional Validation
- [ ] All API endpoints work correctly
- [ ] User authentication and authorization function properly
- [ ] Music catalog operations (CRUD) work as expected
- [ ] Social features (following, posts, comments) operate correctly
- [ ] Playlist and scratch functionality is intact
- [ ] Notification system delivers messages
- [ ] Search functionality returns accurate results
- [ ] File upload to Azure Blob Storage works
- [ ] All existing business logic is preserved

### Technical Validation
- [ ] No Entity Framework dependencies remain
- [ ] MongoDB connections are properly configured
- [ ] Database schema is created automatically on startup
- [ ] Proper indexes are created and utilized
- [ ] Error handling is comprehensive
- [ ] Logging provides adequate information
- [ ] Performance meets or exceeds previous implementation
- [ ] All unit and integration tests pass
- [ ] Code is clean and follows MongoDB best practices

### Production Readiness
- [ ] Environment configurations are secure
- [ ] Connection strings use proper authentication
- [ ] Database backups are configured
- [ ] Monitoring and alerting are in place
- [ ] Documentation is complete and accurate
- [ ] Deployment procedures are tested and documented

## Emergency Rollback Plan

If issues arise during migration:

1. **Immediate Rollback**: Revert to the previous Git commit with Entity Framework
2. **Data Recovery**: Restore from database backups if data migration was attempted
3. **Configuration Reset**: Restore previous connection strings and dependencies
4. **Testing**: Verify all functionality works with the previous implementation

## Success Criteria

The migration is successful when:
- The application runs entirely on MongoDB without any SQL dependencies
- All existing functionality is preserved and working
- Database schema is automatically created and maintained
- Performance meets or exceeds the previous implementation
- The codebase is clean and maintainable
- All tests pass and documentation is complete

---

## Notes for AI Agent

- Follow the prompts sequentially for best results
- Test thoroughly after each phase before proceeding
- Maintain backward compatibility in API contracts
- Keep detailed logs of changes made
- Create Git commits after each major phase for rollback capability
- Focus on preserving existing business logic and functionality
- Prioritize data integrity and consistency throughout the migration
