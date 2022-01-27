import { mergeMap as _observableMergeMap, catchError as _observableCatch, map, catchError } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf, Subject } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpResponse, HttpResponseBase } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';
import { ChatMessageDto, ChatMessagerDto, GetUserChatFriendsWithSettingsOutput } from '../models/Chat/chathub';
import { UtilsService } from 'abp-ng2-module';
import { AppConsts } from '@shared/AppConsts';
export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class ChathubService {

    private encryptedAuthToken = new UtilsService().getCookieValue(AppConsts.authorization.encryptedAuthTokenName);
    urlApi = 'http://localhost:21021';
    public connection: signalR.HubConnection

    recivedMessager: ChatMessageDto;
    messages: ChatMessagerDto[] = [];
    connectionId: string;

    readonly CHAT_POST_URL = this.urlApi + "/api/services/app/chat";

    constructor(private http: HttpClient) {


    }

    public StartConnection() {
        this.connection = new signalR.HubConnectionBuilder().withUrl(this.urlApi + "/messager?" + AppConsts.authorization.encryptedAuthTokenName + '=' + encodeURIComponent(this.encryptedAuthToken)).build();
        this.connection.start().then(() => console.log('Connection started'))
            .then(() => this.getConnectionId())
            .catch(err => console.log('Error while starting connection: ' + err));

    }

    public getConnectionId() {
        this.connection.invoke('GetConnectionId').then(
            (data) => {
                console.log('connectionId', data);
                this.connectionId = data;
            }
        );
    }

    public Send(sendMessageData: ChatMessageDto) {
        this.connection.invoke('SendMessage', sendMessageData)
            .then(() => {

            })
            .catch(err => console.error(err));
    }

    public SendMessageToClient(messages: ChatMessagerDto[]) {
        this.connection.on('SendMessageToClient', (chatHub) => {
            console.log('chatHub', chatHub);
            this.recivedMessager = chatHub;
            this.messages.push(chatHub);
        });

    }


    public getUserChatFriendsWithSettings(): Observable<any> {
        return this.http.get(this.urlApi + "/api/services/app/Chat/GetUserChatFriendsWithSettings").pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }

    public getUserChatMessages(data: number): Observable<any> {

        return this.http.get(this.urlApi + "/api/services/app/Chat/GetUserChatMessages", {
            params: new HttpParams().set('UserId', data)
        }).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }

    public markAllUnreadMessagesOfUserAsRead(data: any): Observable<any> {
        return this.http.post(this.urlApi + "/api/services/app/Chat/MarkAllUnreadMessagesOfUserAsRead", data).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }

    public createFriendshipRequestByUserName(data: any): Observable<any> {
        return this.http.post(this.urlApi + "/api/services/app/Friendship/CreateFriendshipRequestByUserName", data).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }
    public blockUser(data: any): Observable<any> {
        return this.http.post(this.urlApi + "/api/services/app/Friendship/BlockUser", data).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }
    public unblockUser(data: any): Observable<any> {
        return this.http.post(this.urlApi + "/api/services/app/Friendship/UnblockUser", data).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }
    public acceptFriendshipRequest(data: any): Observable<any> {
        return this.http.post(this.urlApi + "/api/services/app/Friendship/AcceptFriendshipRequest", data).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }

}