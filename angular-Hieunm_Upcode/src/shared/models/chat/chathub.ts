export class ChatMessageDto implements IChatMessageDto {
    tenantId: string;
    userId: number;
    senderId: number;
    message: string;
    userName: string;
    profilePictureId: string;

    constructor(data?: IChatMessageDto) {

    }


}


export interface IChatMessageDto {
    tenantId: string | undefined;
    userId: number | undefined;
    message: string | undefined;
    userName: string | undefined;
    profilePictureId: string | undefined;
}

export class GetUserChatFriendsWithSettingsOutput implements IGetUserChatFriendsWithSettingsOutput {
    serverTime: Date;
    friends: FriendDto[];
}

export interface IGetUserChatFriendsWithSettingsOutput {
    serverTime: Date | undefined;
    friends: FriendDto[] | undefined;
}

export class FriendDto {
    friendUserId?: number;
    friendTenantId?: number;
    friendUserName?: string;
    friendTenancyName?: string;
    friendProfilePictureId?: string;
    unreadMessageCount?: string;
    isOnline?: boolean;
    state?: number;
}

export class ChatMessagerDto {
    tenantId?: string;
    userId?: number;
    message?: string;
    targetUserId?: number;
    side?: number;
    readState?: number;
    creationTime?: Date;
}

