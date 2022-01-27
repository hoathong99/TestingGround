using Abp;

namespace MHPQ.Friendships.Cache
{
    public interface IUserFriendsCache
    {
        UserWithFriendsCacheItem GetCacheItem(UserIdentifier userIdentifier);

        UserWithFriendsCacheItem GetCacheItemOrNull(UserIdentifier userIdentifier);
        UserWithFriendsCacheItem GetUserFriendsCacheItemInternal(UserIdentifier userIdentifier, FriendshipState state);

        void ResetUnreadMessageCount(UserIdentifier userIdentifier, UserIdentifier friendIdentifier);

        void IncreaseUnreadMessageCount(UserIdentifier userIdentifier, UserIdentifier friendIdentifier, int change);

        void AddFriend(UserIdentifier userIdentifier, FriendCacheItem friend);

        void RemoveFriend(UserIdentifier userIdentifier, FriendCacheItem friend);

        void UpdateFriend(UserIdentifier userIdentifier, FriendCacheItem friend);
    }
}
