# MongoDB Schema Strategy for Scratchy Application

## Overview

This document defines the complete MongoDB schema strategy for the Scratchy application, converting the existing SQL Server Entity Framework model to a MongoDB document-based architecture optimized for performance and scalability.

## Schema Design Principles

### 1. Document Pattern Strategy
- **Embed frequently accessed together**: User settings, album tracks, post comments
- **Reference for independent access**: Artists, albums, users (main entities)
- **Denormalize for performance**: Username, profile pictures, artist names
- **Separate for large collections**: Posts, scratches, notifications

### 2. Collection Naming Convention
- Lowercase, plural nouns: `users`, `artists`, `albums`
- Clear, descriptive names
- Consistent with REST API endpoints

### 3. Field Naming Convention
- camelCase for field names
- Consistent with JavaScript/JSON standards
- Clear, descriptive field names

---

## Detailed Collection Schemas

### 1. **users** Collection

#### Document Structure
```json
{
  "_id": ObjectId("..."),
  "firebaseId": "firebase_user_123",
  "username": "johndoe",
  "email": "john.doe@example.com",
  "passwordHash": "bcrypt_hash_here",
  "profilePictureUrl": "https://example.com/profile.jpg",
  "createdAt": ISODate("2024-01-15T10:30:00.000Z"),
  "updatedAt": ISODate("2024-01-15T10:30:00.000Z"),
  
  // Embedded Settings (1:1 relationship)
  "settings": {
    "isPublicProfile": true,
    "receiveNotifications": true,
    "language": "en",
    "theme": "dark"
  },
  
  // Embedded Badges (M:M via UserBadge)
  "badges": [
    {
      "badgeId": ObjectId("..."),
      "name": "First Post",
      "description": "Created your first post",
      "iconUrl": "https://example.com/badge1.png",
      "awardedAt": ISODate("2024-01-15T11:00:00.000Z")
    }
  ],
  
  // Computed/Cached Fields
  "stats": {
    "followersCount": 150,
    "followingCount": 75,
    "postsCount": 42,
    "scratchesCount": 38,
    "playlistsCount": 5
  }
}
```

#### Field Types & Validation
- `_id`: ObjectId (auto-generated)
- `firebaseId`: String, required, unique, maxLength: 50
- `username`: String, required, unique, maxLength: 50, pattern: /^[a-zA-Z0-9_]+$/
- `email`: String, optional, unique (sparse), maxLength: 100, email format
- `passwordHash`: String, optional, maxLength: 256
- `profilePictureUrl`: String, optional, maxLength: 255, URL format
- `createdAt`: Date, required, default: now
- `updatedAt`: Date, required, default: now
- `settings.*`: Object, required with defaults
- `badges`: Array of objects, default: []
- `stats.*`: Numbers, default: 0

#### Indexes
```javascript
// Unique indexes
db.users.createIndex({ "firebaseId": 1 }, { unique: true })
db.users.createIndex({ "username": 1 }, { unique: true })
db.users.createIndex({ "email": 1 }, { unique: true, sparse: true })

// Query performance indexes
db.users.createIndex({ "username": "text" }) // Text search
db.users.createIndex({ "createdAt": -1 }) // Newest users first
db.users.createIndex({ "stats.followersCount": -1 }) // Popular users
```

#### Validation Schema
```javascript
db.createCollection("users", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["firebaseId", "username", "createdAt", "settings"],
      properties: {
        firebaseId: { bsonType: "string", maxLength: 50 },
        username: { 
          bsonType: "string", 
          maxLength: 50,
          pattern: "^[a-zA-Z0-9_]+$"
        },
        email: { 
          bsonType: ["string", "null"], 
          maxLength: 100,
          pattern: "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$"
        },
        settings: {
          bsonType: "object",
          required: ["isPublicProfile", "receiveNotifications", "language", "theme"],
          properties: {
            isPublicProfile: { bsonType: "bool" },
            receiveNotifications: { bsonType: "bool" },
            language: { bsonType: "string", maxLength: 10 },
            theme: { bsonType: "string", maxLength: 20 }
          }
        }
      }
    }
  }
})
```

---

### 2. **artists** Collection

#### Document Structure
```json
{
  "_id": ObjectId("..."),
  "spotifyId": "spotify_artist_123",
  "name": "The Beatles",
  "bio": "Legendary British rock band...",
  "profilePictureUrl": "https://example.com/artist.jpg",
  "createdAt": ISODate("2024-01-15T10:30:00.000Z"),
  "updatedAt": ISODate("2024-01-15T10:30:00.000Z"),
  
  // Cached aggregated data
  "stats": {
    "albumsCount": 13,
    "postsCount": 1250,
    "scratchesCount": 890,
    "followersCount": 50000
  }
}
```

#### Indexes
```javascript
db.artists.createIndex({ "spotifyId": 1 }, { unique: true })
db.artists.createIndex({ "name": "text" }) // Text search
db.artists.createIndex({ "name": 1 }) // Alphabetical sorting
db.artists.createIndex({ "stats.followersCount": -1 }) // Popular artists
db.artists.createIndex({ "createdAt": -1 }) // Newest artists
```

---

### 3. **albums** Collection

#### Document Structure
```json
{
  "_id": ObjectId("..."),
  "title": "Abbey Road",
  "spotifyId": "spotify_album_456",
  "artistId": ObjectId("..."),
  "artistName": "The Beatles", // Denormalized for performance
  "releaseDate": ISODate("1969-09-26T00:00:00.000Z"),
  "coverImageUrl": "https://example.com/abbey-road.jpg",
  "nfcTagId": "nfc_12345",
  "createdAt": ISODate("2024-01-15T10:30:00.000Z"),
  "updatedAt": ISODate("2024-01-15T10:30:00.000Z"),
  
  // Embedded Genres (M:M converted to array)
  "genres": ["Rock", "Pop", "Psychedelic"],
  
  // Embedded Tracks (1:M relationship)
  "tracks": [
    {
      "_id": ObjectId("..."),
      "title": "Come Together",
      "duration": "PT4M20S", // ISO 8601 duration format
      "trackNumber": 1
    },
    {
      "_id": ObjectId("..."),
      "title": "Something",
      "duration": "PT3M3S",
      "trackNumber": 2
    }
  ],
  
  // Cached aggregated data
  "stats": {
    "tracksCount": 17,
    "postsCount": 245,
    "scratchesCount": 189,
    "averageRating": 4.7,
    "totalDuration": "PT47M23S"
  }
}
```

#### Indexes
```javascript
db.albums.createIndex({ "spotifyId": 1 }, { unique: true, sparse: true })
db.albums.createIndex({ "artistId": 1, "releaseDate": -1 }) // Artist's albums by date
db.albums.createIndex({ "title": "text", "artistName": "text" }) // Text search
db.albums.createIndex({ "genres": 1 }) // Genre filtering
db.albums.createIndex({ "releaseDate": -1 }) // Newest albums
db.albums.createIndex({ "stats.averageRating": -1 }) // Top rated albums
db.albums.createIndex({ "nfcTagId": 1 }, { sparse: true }) // NFC tag lookup
db.albums.createIndex({ "tracks._id": 1 }) // Track lookup within albums
```

---

### 4. **posts** Collection

#### Document Structure
```json
{
  "_id": ObjectId("..."),
  "userId": ObjectId("..."),
  "username": "johndoe", // Denormalized
  "userProfilePicture": "https://example.com/profile.jpg", // Denormalized
  "albumId": ObjectId("..."),
  "albumTitle": "Abbey Road", // Denormalized
  "albumCoverUrl": "https://example.com/abbey-road.jpg", // Denormalized
  "artistName": "The Beatles", // Denormalized
  "content": "This album is absolutely incredible! The production quality is amazing.",
  "rating": 4.5,
  "createdAt": ISODate("2024-01-15T10:30:00.000Z"),
  "updatedAt": ISODate("2024-01-15T10:30:00.000Z"),
  
  // Embedded Comments (1:M relationship, limited to recent comments)
  "comments": [
    {
      "_id": ObjectId("..."),
      "userId": ObjectId("..."),
      "username": "janedoe", // Denormalized
      "userProfilePicture": "https://example.com/jane.jpg", // Denormalized
      "content": "I totally agree! Best album ever.",
      "createdAt": ISODate("2024-01-15T11:00:00.000Z")
    }
  ],
  
  // Stats and metadata
  "stats": {
    "commentsCount": 15,
    "likesCount": 89,
    "sharesCount": 12
  },
  
  // For large comment lists, store only recent comments inline
  "hasMoreComments": true
}
```

#### Indexes
```javascript
db.posts.createIndex({ "userId": 1, "createdAt": -1 }) // User's posts timeline
db.posts.createIndex({ "albumId": 1, "createdAt": -1 }) // Album's posts
db.posts.createIndex({ "createdAt": -1 }) // Global feed
db.posts.createIndex({ "content": "text" }) // Text search in posts
db.posts.createIndex({ "rating": -1 }) // Highest rated posts
db.posts.createIndex({ "stats.likesCount": -1 }) // Most liked posts
db.posts.createIndex({ "comments.userId": 1 }) // User's comments
```

---

### 5. **scratches** Collection

#### Document Structure
```json
{
  "_id": ObjectId("..."),
  "userId": ObjectId("..."),
  "username": "johndoe", // Denormalized
  "userProfilePicture": "https://example.com/profile.jpg", // Denormalized
  "albumId": ObjectId("..."),
  "albumTitle": "Abbey Road", // Denormalized
  "albumCoverUrl": "https://example.com/abbey-road.jpg", // Denormalized
  "artistName": "The Beatles", // Denormalized
  "title": "My Abbey Road Experience",
  "content": "Discovered this gem through vinyl hunting...",
  "rating": 5,
  "likeCounter": 42,
  "scratchImageUrl": "https://example.com/scratch1.jpg",
  "createdAt": ISODate("2024-01-15T10:30:00.000Z"),
  "updatedAt": ISODate("2024-01-15T10:30:00.000Z"),
  
  // Embedded Tags (M:M converted to array)
  "tags": ["vinyl", "classic-rock", "british-invasion", "1969"],
  
  // Stats
  "stats": {
    "viewsCount": 234,
    "sharesCount": 8,
    "commentsCount": 12
  }
}
```

#### Indexes
```javascript
db.scratches.createIndex({ "userId": 1, "createdAt": -1 }) // User's scratches
db.scratches.createIndex({ "albumId": 1, "createdAt": -1 }) // Album's scratches
db.scratches.createIndex({ "tags": 1 }) // Tag filtering
db.scratches.createIndex({ "title": "text", "content": "text" }) // Text search
db.scratches.createIndex({ "likeCounter": -1 }) // Most liked scratches
db.scratches.createIndex({ "rating": -1 }) // Highest rated scratches
db.scratches.createIndex({ "createdAt": -1 }) // Global scratches feed
```

---

### 6. **playlists** Collection

#### Document Structure
```json
{
  "_id": ObjectId("..."),
  "userId": ObjectId("..."),
  "username": "johndoe", // Denormalized
  "title": "My Favorite Rock Classics",
  "description": "The best rock songs from the 60s and 70s",
  "isPublic": true,
  "createdAt": ISODate("2024-01-15T10:30:00.000Z"),
  "updatedAt": ISODate("2024-01-15T10:30:00.000Z"),
  
  // Embedded Tracks (M:M converted to embedded array)
  "tracks": [
    {
      "trackId": ObjectId("..."),
      "title": "Come Together",
      "albumId": ObjectId("..."),
      "albumTitle": "Abbey Road",
      "artistName": "The Beatles",
      "duration": "PT4M20S",
      "addedAt": ISODate("2024-01-15T10:30:00.000Z"),
      "position": 1
    },
    {
      "trackId": ObjectId("..."),
      "title": "Stairway to Heaven",
      "albumId": ObjectId("..."),
      "albumTitle": "Led Zeppelin IV",
      "artistName": "Led Zeppelin",
      "duration": "PT8M2S",
      "addedAt": ISODate("2024-01-15T11:00:00.000Z"),
      "position": 2
    }
  ],
  
  // Calculated fields
  "stats": {
    "tracksCount": 25,
    "totalDuration": "PT2H15M30S",
    "followersCount": 89,
    "playsCount": 1250
  }
}
```

#### Indexes
```javascript
db.playlists.createIndex({ "userId": 1, "createdAt": -1 }) // User's playlists
db.playlists.createIndex({ "isPublic": 1, "stats.followersCount": -1 }) // Popular public playlists
db.playlists.createIndex({ "title": "text", "description": "text" }) // Text search
db.playlists.createIndex({ "tracks.trackId": 1 }) // Track membership lookup
db.playlists.createIndex({ "tracks.albumId": 1 }) // Album in playlists
db.playlists.createIndex({ "createdAt": -1 }) // Newest playlists
```

---

### 7. **follows** Collection

#### Document Structure
```json
{
  "_id": ObjectId("..."),
  "followerId": ObjectId("..."),
  "followerUsername": "johndoe", // Denormalized
  "followedId": ObjectId("..."),
  "followedUsername": "janedoe", // Denormalized
  "createdAt": ISODate("2024-01-15T10:30:00.000Z"),
  
  // Additional metadata
  "isNotificationEnabled": true,
  "followType": "user" // user, artist, etc.
}
```

#### Indexes
```javascript
db.follows.createIndex({ "followerId": 1, "createdAt": -1 }) // Who user follows
db.follows.createIndex({ "followedId": 1, "createdAt": -1 }) // User's followers
db.follows.createIndex({ "followerId": 1, "followedId": 1 }, { unique: true }) // Prevent duplicates
db.follows.createIndex({ "followedId": 1, "followerId": 1 }) // Reverse lookup
```

---

### 8. **notifications** Collection

#### Document Structure
```json
{
  "_id": ObjectId("..."),
  "userId": ObjectId("..."),
  "senderId": ObjectId("..."),
  "senderUsername": "janedoe", // Denormalized
  "senderProfilePicture": "https://example.com/jane.jpg", // Denormalized
  "type": "follow", // follow, like, comment, mention, system
  "title": "New Follower",
  "message": "janedoe started following you",
  "isRead": false,
  "createdAt": ISODate("2024-01-15T10:30:00.000Z"),
  
  // Auto-cleanup after 30 days
  "expiresAt": ISODate("2024-02-14T10:30:00.000Z"),
  
  // Related entity references
  "relatedEntity": {
    "type": "user", // user, post, scratch, album
    "id": ObjectId("..."),
    "title": "User Profile" // Contextual title
  },
  
  // Action metadata
  "actionData": {
    "postId": ObjectId("..."), // If related to a post
    "albumId": ObjectId("..."), // If related to an album
    "customData": {} // Flexible data for different notification types
  }
}
```

#### Indexes
```javascript
db.notifications.createIndex({ "userId": 1, "isRead": 1, "createdAt": -1 }) // User's notifications
db.notifications.createIndex({ "expiresAt": 1 }, { expireAfterSeconds: 0 }) // TTL index
db.notifications.createIndex({ "senderId": 1, "createdAt": -1 }) // Sent notifications
db.notifications.createIndex({ "type": 1, "createdAt": -1 }) // Notification type filtering
```

---

### 9. **badges** Collection (Reference Data)

#### Document Structure
```json
{
  "_id": ObjectId("..."),
  "name": "First Post",
  "description": "Created your first post on Scratchy",
  "iconUrl": "https://example.com/badges/first-post.png",
  "category": "milestone", // milestone, achievement, special
  "rarity": "common", // common, rare, epic, legendary
  "isActive": true,
  "createdAt": ISODate("2024-01-15T10:30:00.000Z"),
  
  // Badge criteria (for automatic awarding)
  "criteria": {
    "type": "post_count",
    "threshold": 1,
    "conditions": {}
  }
}
```

#### Indexes
```javascript
db.badges.createIndex({ "name": 1 }, { unique: true })
db.badges.createIndex({ "category": 1, "rarity": 1 })
db.badges.createIndex({ "isActive": 1 })
```

---

## Query Patterns and Performance Considerations

### 1. Common Query Patterns

#### User Feed Generation
```javascript
// Get posts from followed users
db.posts.aggregate([
  {
    $match: {
      userId: { $in: followedUserIds },
      createdAt: { $gte: lastWeek }
    }
  },
  { $sort: { createdAt: -1 } },
  { $limit: 20 }
])
```

#### Album Discovery
```javascript
// Find albums by genre with high ratings
db.albums.find({
  genres: { $in: ["Rock", "Pop"] },
  "stats.averageRating": { $gte: 4.0 }
}).sort({ "stats.averageRating": -1 }).limit(10)
```

#### Search Functionality
```javascript
// Text search across multiple collections
db.albums.find({
  $text: { $search: "beatles abbey road" }
}).sort({ score: { $meta: "textScore" } })
```

### 2. Data Consistency Strategy

#### Eventual Consistency Areas
- User follower/following counts
- Post/scratch like counts  
- Album rating averages
- Notification counters

#### Strong Consistency Requirements
- User authentication data
- Follow relationships
- Financial transactions
- Critical user settings

#### Update Patterns
```javascript
// Example: Update denormalized username across collections
// Use MongoDB transactions for critical updates
session.withTransaction(async () => {
  await db.users.updateOne({_id: userId}, {$set: {username: newUsername}})
  await db.posts.updateMany({userId: userId}, {$set: {username: newUsername}})
  await db.scratches.updateMany({userId: userId}, {$set: {username: newUsername}})
  // ... other collections
})
```

### 3. Aggregation Pipeline Examples

#### User Statistics
```javascript
db.users.aggregate([
  {
    $lookup: {
      from: "posts",
      localField: "_id",
      foreignField: "userId",
      as: "posts"
    }
  },
  {
    $lookup: {
      from: "follows",
      localField: "_id", 
      foreignField: "followedId",
      as: "followers"
    }
  },
  {
    $addFields: {
      "stats.postsCount": { $size: "$posts" },
      "stats.followersCount": { $size: "$followers" }
    }
  }
])
```

## Transaction Strategy

### When to Use Transactions
1. **User Registration**: Create user + initial settings
2. **Follow/Unfollow**: Update follow relationship + user counts
3. **Post Creation**: Create post + update user stats
4. **Username Changes**: Update denormalized data across collections
5. **Badge Awards**: Update user + create notification

### Transaction Example
```javascript
const session = client.startSession()
try {
  await session.withTransaction(async () => {
    // Create user
    await db.users.insertOne(userData, { session })
    
    // Create default settings
    await db.users.updateOne(
      { _id: userData._id },
      { $set: { settings: defaultSettings } },
      { session }
    )
    
    // Send welcome notification
    await db.notifications.insertOne(welcomeNotification, { session })
  })
} finally {
  await session.endSession()
}
```

## Migration Considerations

### Data Size Optimization
- **Document Size Limits**: Keep documents under 16MB
- **Array Size Limits**: Limit embedded arrays (comments, tracks) to reasonable sizes
- **Pagination Strategy**: Use cursor-based pagination for large result sets

### Performance Monitoring
- **Index Usage**: Monitor query execution plans
- **Document Growth**: Track document size growth over time
- **Query Performance**: Monitor slow query logs
- **Memory Usage**: Track working set size

This schema strategy provides a solid foundation for the MongoDB migration, balancing performance, consistency, and maintainability while leveraging MongoDB's document-oriented strengths.
