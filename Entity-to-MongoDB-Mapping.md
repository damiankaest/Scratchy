# Entity Framework to MongoDB Data Model Mapping Analysis

## Current SQL Entity Analysis

Based on the examination of the ScratchItDbContext and entity models, here is the complete analysis of the current data structure and the proposed MongoDB document design.

## 1. Current SQL Entities and Their Properties

### Core Entities

#### User Entity
- **Primary Key**: UserId (int)
- **Properties**: 
  - FirebaseId (string, 50 chars)
  - Username (string, 50 chars)
  - Email (string, 100 chars)
  - PasswordHash (string, 256 chars)
  - ProfilePictureUrl (string, 255 chars)
  - CreatedAt (DateTime)
- **Navigation Properties**: Followers, Followings, Posts, Comments, UserImages, ReceivedNotifications, SentNotifications, Settings, Scratches, Playlists, UserBadges, ShowCases

#### Artist Entity
- **Primary Key**: ArtistId (int)
- **Properties**:
  - SpotifyId (string)
  - Name (string, 100 chars)
  - Bio (string)
  - ProfilePictureUrl (string, 255 chars)
  - CreatedAt (DateTime)
- **Navigation Properties**: Albums

#### Album Entity
- **Primary Key**: AlbumId (int)
- **Foreign Keys**: ArtistId
- **Properties**:
  - Title (string, 100 chars)
  - SpotifyId (string, 100 chars)
  - ReleaseDate (DateTime?)
  - CoverImageUrl (string, 255 chars)
  - NfcTagId (string, 50 chars)
  - CreatedAt (DateTime)
- **Navigation Properties**: Artist, Tracks, Genres (M:M), Posts, Scratches

#### Track Entity
- **Primary Key**: TrackId (int)
- **Foreign Keys**: AlbumId
- **Properties**:
  - Title (string, 100 chars)
  - Duration (TimeSpan?)
  - TrackNumber (int)
- **Navigation Properties**: Album, Playlists (M:M)

### Social Features

#### Post Entity
- **Primary Key**: PostId (int)
- **Foreign Keys**: UserId, AlbumId (nullable)
- **Properties**:
  - Content (string)
  - Rating (decimal(2,1)?)
  - CreatedAt (DateTime)
- **Navigation Properties**: User, Album, Comments

#### Comment Entity
- **Primary Key**: CommentId (int)
- **Foreign Keys**: PostId, UserId
- **Properties**:
  - Content (string)
  - CreatedAt (DateTime)
- **Navigation Properties**: Post, User

#### Follow Entity (Junction Table)
- **Composite Key**: FollowerId + FollowedId
- **Properties**:
  - CreatedAt (DateTime)
- **Navigation Properties**: Follower (User), Followed (User)

#### Scratch Entity
- **Primary Key**: ScratchId (int)
- **Foreign Keys**: UserId, AlbumId (nullable)
- **Properties**:
  - Title (string, 100 chars)
  - Content (string)
  - Rating (int)
  - LikeCounter (int)
  - ScratchImageUrl (string?)
  - CreatedAt (DateTime)
- **Navigation Properties**: User, Album, Tags (M:M)

### Playlist and Music Organization

#### Playlist Entity
- **Primary Key**: PlaylistId (int)
- **Foreign Keys**: UserId
- **Properties**:
  - Title (string, 100 chars)
  - Description (string)
  - CreatedAt (DateTime)
- **Navigation Properties**: User, Tracks (M:M)

#### Tag Entity
- **Primary Key**: TagId (int)
- **Properties**:
  - Name (string, 50 chars)
- **Navigation Properties**: Scratches (M:M)

#### Genre Entity
- **Primary Key**: GenreId (int)
- **Properties**:
  - Name (string, 50 chars)
- **Navigation Properties**: Albums (M:M)

### System Entities

#### Notification Entity
- **Primary Key**: NotificationId (int)
- **Foreign Keys**: UserId, SenderId
- **Properties**:
  - Type (string, 50 chars)
  - Message (string)
  - IsRead (bool)
  - CreatedAt (DateTime)
- **Navigation Properties**: User, Sender (User)

#### Settings Entity
- **Primary Key**: SettingsId (int)
- **Foreign Keys**: UserId (1:1)
- **Properties**:
  - IsPublicProfile (bool)
  - ReceiveNotifications (bool)
  - Language (string, 10 chars)
  - Theme (string, 20 chars)

#### Badge Entity
- **Primary Key**: Id (int)
- **Properties**:
  - Name (string, 100 chars)
  - Description (string)
  - IconUrl (string)
- **Navigation Properties**: UserBadges

#### UserBadge Entity (Junction Table)
- **Primary Key**: Id (int)
- **Foreign Keys**: UserId, BadgeId
- **Properties**:
  - AwardedAt (DateTime)
- **Navigation Properties**: User, Badge

### Junction Tables
- **ScratchesTags**: ScratchId + TagId (composite key)
- **AlbumGenres**: AlbumId + GenreId (composite key)
- **PlaylistTracks**: PlaylistId + TrackId (composite key)

## 2. Entity Relationships Analysis

### One-to-Many Relationships
- User → Posts (1:M)
- User → Comments (1:M)
- User → Scratches (1:M)
- User → Playlists (1:M)
- User → ReceivedNotifications (1:M)
- User → SentNotifications (1:M)
- Artist → Albums (1:M)
- Album → Tracks (1:M)
- Album → Posts (1:M)
- Album → Scratches (1:M)
- Post → Comments (1:M)

### One-to-One Relationships
- User ↔ Settings (1:1)

### Many-to-Many Relationships
- User ↔ User (Follow relationship)
- Scratch ↔ Tag (via ScratchesTags)
- Album ↔ Genre (via AlbumGenres)
- Playlist ↔ Track (via PlaylistTracks)
- User ↔ Badge (via UserBadge)

### Self-Referencing Relationships
- User → User (Follow: Follower/Followed)
- Notification: User → User (User/Sender)

## 3. Proposed MongoDB Document Structure

### Collection Design Strategy

#### **users** Collection
```json
{
  "_id": ObjectId,
  "firebaseId": "string",
  "username": "string",
  "email": "string",
  "passwordHash": "string",
  "profilePictureUrl": "string",
  "createdAt": ISODate,
  "settings": {
    "isPublicProfile": boolean,
    "receiveNotifications": boolean,
    "language": "string",
    "theme": "string"
  },
  "badges": [
    {
      "badgeId": ObjectId,
      "name": "string",
      "description": "string",
      "iconUrl": "string",
      "awardedAt": ISODate
    }
  ],
  "followersCount": number,
  "followingCount": number
}
```

#### **artists** Collection
```json
{
  "_id": ObjectId,
  "spotifyId": "string",
  "name": "string",
  "bio": "string",
  "profilePictureUrl": "string",
  "createdAt": ISODate,
  "albumsCount": number
}
```

#### **albums** Collection
```json
{
  "_id": ObjectId,
  "title": "string",
  "spotifyId": "string",
  "artistId": ObjectId,
  "artistName": "string", // denormalized for performance
  "releaseDate": ISODate,
  "coverImageUrl": "string",
  "nfcTagId": "string",
  "createdAt": ISODate,
  "genres": ["string"],
  "tracks": [
    {
      "trackId": ObjectId,
      "title": "string",
      "duration": "string", // ISO 8601 duration format
      "trackNumber": number
    }
  ],
  "postsCount": number,
  "scratchesCount": number
}
```

#### **posts** Collection
```json
{
  "_id": ObjectId,
  "userId": ObjectId,
  "username": "string", // denormalized
  "userProfilePicture": "string", // denormalized
  "albumId": ObjectId,
  "albumTitle": "string", // denormalized
  "albumCoverUrl": "string", // denormalized
  "artistName": "string", // denormalized
  "content": "string",
  "rating": number,
  "createdAt": ISODate,
  "commentsCount": number,
  "comments": [
    {
      "_id": ObjectId,
      "userId": ObjectId,
      "username": "string",
      "userProfilePicture": "string",
      "content": "string",
      "createdAt": ISODate
    }
  ]
}
```

#### **scratches** Collection
```json
{
  "_id": ObjectId,
  "userId": ObjectId,
  "username": "string", // denormalized
  "userProfilePicture": "string", // denormalized
  "albumId": ObjectId,
  "albumTitle": "string", // denormalized
  "albumCoverUrl": "string", // denormalized
  "artistName": "string", // denormalized
  "title": "string",
  "content": "string",
  "rating": number,
  "likeCounter": number,
  "scratchImageUrl": "string",
  "createdAt": ISODate,
  "tags": ["string"]
}
```

#### **playlists** Collection
```json
{
  "_id": ObjectId,
  "userId": ObjectId,
  "username": "string", // denormalized
  "title": "string",
  "description": "string",
  "createdAt": ISODate,
  "tracks": [
    {
      "trackId": ObjectId,
      "title": "string",
      "albumId": ObjectId,
      "albumTitle": "string",
      "artistName": "string",
      "duration": "string",
      "addedAt": ISODate
    }
  ],
  "trackCount": number
}
```

#### **follows** Collection
```json
{
  "_id": ObjectId,
  "followerId": ObjectId,
  "followerUsername": "string", // denormalized
  "followedId": ObjectId,
  "followedUsername": "string", // denormalized
  "createdAt": ISODate
}
```

#### **notifications** Collection
```json
{
  "_id": ObjectId,
  "userId": ObjectId,
  "senderId": ObjectId,
  "senderUsername": "string", // denormalized
  "senderProfilePicture": "string", // denormalized
  "type": "string",
  "message": "string",
  "isRead": boolean,
  "createdAt": ISODate,
  "expiresAt": ISODate // TTL index for auto-cleanup
}
```

#### **badges** Collection (Reference Collection)
```json
{
  "_id": ObjectId,
  "name": "string",
  "description": "string",
  "iconUrl": "string",
  "isActive": boolean
}
```

## 4. MongoDB Relationship Strategy

### Embedded vs Referenced Documents

#### **Embedded Documents** (Denormalized):
- **User Settings**: Embedded in user document (1:1 relationship, rarely changes)
- **User Badges**: Embedded in user document (small list, infrequently updated)
- **Album Tracks**: Embedded in album document (logical grouping, accessed together)
- **Post Comments**: Embedded in posts (limited size, accessed together)
- **Playlist Tracks**: Embedded in playlist document (logical grouping)

#### **Referenced Documents** (Normalized):
- **Artist → Albums**: Keep separate for independent queries
- **User → Posts/Scratches**: Keep separate for pagination and independent access
- **User → User (Follows)**: Separate collection for relationship management
- **Notifications**: Separate collection with TTL for auto-cleanup

#### **Hybrid Approach** (Denormalized References):
- Store reference IDs + frequently accessed fields (username, profilePicture, etc.)
- Reduces need for joins while maintaining data integrity

## 5. Index Requirements

### Primary Indexes (Unique)
- `users.firebaseId` (unique)
- `users.username` (unique)
- `users.email` (unique, sparse)
- `artists.spotifyId` (unique)
- `albums.spotifyId` (unique, sparse)

### Query Performance Indexes
- `posts.userId` + `posts.createdAt` (compound, for user feeds)
- `posts.albumId` + `posts.createdAt` (compound, for album posts)
- `scratches.userId` + `scratches.createdAt` (compound, for user scratches)
- `scratches.albumId` + `scratches.createdAt` (compound, for album scratches)
- `follows.followerId` + `follows.createdAt` (compound)
- `follows.followedId` + `follows.createdAt` (compound)
- `notifications.userId` + `notifications.isRead` + `notifications.createdAt` (compound)
- `albums.artistId` (for artist's albums)
- `playlists.userId` + `playlists.createdAt` (compound)

### Text Search Indexes
- `users.username` (text search)
- `artists.name` (text search)
- `albums.title` (text search)
- `posts.content` (text search)
- `scratches.title` + `scratches.content` (compound text search)

### TTL Indexes
- `notifications.expiresAt` (TTL index for auto-cleanup after 30 days)

## 6. Data Consistency Considerations

### Eventual Consistency Areas
- User follower/following counts
- Album posts/scratches counts
- Post comment counts
- User badge updates

### Strong Consistency Requirements
- User authentication data
- Financial/critical user settings
- Follow relationships (to prevent duplicate follows)

### Data Integrity Patterns
- **Denormalization Updates**: Use MongoDB transactions for updating denormalized data
- **Counters**: Use separate counter updates with retry logic
- **Cascading Deletes**: Handle in application logic (user deletion → posts/scratches cleanup)

## 7. Migration Strategy Recommendations

### Phase 1: Core Collections
1. Users (with embedded settings and badges)
2. Artists
3. Albums (with embedded tracks)

### Phase 2: Social Features
1. Posts (with embedded comments)
2. Scratches
3. Follows

### Phase 3: Additional Features
1. Playlists
2. Notifications
3. Badges reference collection

### Data Migration Approach
- **Dual Write**: Write to both SQL and MongoDB during transition
- **Background Sync**: Sync existing data in batches
- **Validation**: Compare data consistency between systems
- **Cutover**: Switch reads to MongoDB once validated

This analysis provides the foundation for implementing a MongoDB-only architecture that maintains all existing functionality while leveraging MongoDB's document-oriented benefits for better performance and scalability.
