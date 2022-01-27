import { AfterViewChecked, Component, ElementRef, Injector, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Output, EventEmitter } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ChatMessageDto, ChatMessagerDto, FriendDto, GetUserChatFriendsWithSettingsOutput } from '@shared/models/chat/chathub';
import { ChathubService } from '@shared/service-proxies/chathub.service';
import { AppSessionService } from '@shared/session/app-session.service';
import { LocalizationService, TokenService, UtilsService } from 'abp-ng2-module';
import * as _ from 'lodash';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import * as signalR from '@aspnet/signalr';
import { Subject } from '@microsoft/signalr';
import { AppComponentBase } from '@shared/app-component-base';
import { AppConsts } from '@shared/AppConsts';



export interface UserMes {
  id?: number;
  userName?: string;
  state?: string;
  imageurl?: string;
  numberMes?: number;
}

export interface Messager {
  id?: number;
  userName?: string;
  userId?: number;
  imageurl?: string;
  time?: string;
  message?: string;
}

@Component({
  selector: 'app-messager',
  templateUrl: './messager.component.html',
  styleUrls: ['./messager.component.css'],
  animations: [appModuleAnimation()]
})
export class MessagerComponent extends AppComponentBase implements OnInit, OnChanges, AfterViewChecked {

  myId = this.appSession.userId;
  myName = this.appSession.user.userName;

  chathub = new ChatMessageDto();
  msgInboxArray: ChatMessageDto[] = [];
  serverClientTimeDifference = 0;
  selectedUser: FriendDto = { };
  currentUser = this.appSessionService.user;
  isOpen = false;
  pinned = false;
  sendingMessage = false;
  loadingPreviousUserMessages = false;
  userNameFilter = '';
  messages: ChatMessagerDto[] = [];
  vm = this;

  userChatFriendsWithSettingsOutput: GetUserChatFriendsWithSettingsOutput;
  friends: FriendDto[];

  userMes: UserMes[] = [];
  userSeclect: FriendDto = { };
  messagers: Messager[] = [];

  receivedMessageObject: ChatMessageDto = new ChatMessageDto();
  sendMessageData: ChatMessageDto = new ChatMessageDto();
  sharedObj = new Subject<ChatMessageDto>();




  constructor(
    injector: Injector,
    private router: Router,
    private chatService: ChathubService,
    private appSessionService: AppSessionService,
    private localizationService: LocalizationService,
    private http: HttpClient,
    private _tokenService: TokenService,
  ) {

    super(injector);


  }
  ngOnChanges(changes: SimpleChanges): void {
    console.log('changes', changes);
  }

  @Output() newItemEvent = new EventEmitter<boolean>();

  @ViewChild('scrollMe') private myScrollContainer: ElementRef;


  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  scrollToBottom(): void {
    try {
      this.myScrollContainer.nativeElement.scrollTop = this.myScrollContainer.nativeElement.scrollHeight;
    } catch (err) { }
  }

  ngOnInit() {
    this.chatService.StartConnection();
    this.chatService.connection.on('SendMessageToClient', (chatHub) => {
      console.log('chatHub', chatHub);
      this.messages.push(chatHub);
    });
    //this.chatService.retrieveMappedObject().subscribe((receivedObj: ChatHub) => { this.addToInbox(receivedObj); });  // calls the service method to get the new messages sent
    this.getJSON('./assets/user.json').subscribe(res => {
      this.userMes = res as any;
    });
    this.getJSON('./assets/message.json').subscribe(res => this.messagers = res as any);
    this.loadData();
    this.scrollToBottom();
  }

  public async start() {
    try {
      await
        console.log("connected a");
    } catch (err) {
      console.log(err);
      setTimeout(() => this.start(), 10000);
    }
  }

  public getJSON(url) {
    return this.http.get(url);
  }

  public loadData() {
    //this.getAllMessages();
    this.getFriends();
  }

  getAllMessages(id: number) {
    console.log('id', id);
    return this.chatService.getUserChatMessages(id).subscribe(
      res => {
        if (res.success) {
          this.messages = res.result.items;
          console.log('messages', this.messages);
        }
        else {

        }
      });
  }

  close(value: boolean) {
    this.newItemEvent.emit(value);
  }

  getmes(user: FriendDto) {
    this.selectedUser = user;
    this.getAllMessages(user.friendUserId);
  }

  send(): void {
    if (this.chathub) {
      if (!this.chathub.userId) {
        window.alert("Both fields are required.");
        return;
      } else {
        //this.chatService.broadcastMessage(this.chathub);                   // Send the message via a service
      }
    }
  }


  sendMessage() {

    this.sendDirectMessage()
  }

  sendDirectMessage() {
    this.sendMessageData.senderId = this.myId;
    this.sendMessageData.userId = this.selectedUser.friendUserId;
    this.sendMessageData.userName = this.selectedUser.friendUserName;
    this.chatService.Send(this.sendMessageData);
    this.sendMessageData.message = '';
  }


  addToInbox(obj: ChatMessageDto) {
    let newObj = new ChatMessageDto();
    newObj.userName = obj.userName;
    newObj.message = obj.message;
    this.msgInboxArray.push(newObj);

  }

  getFriends() {
    return this.chatService.getUserChatFriendsWithSettings().subscribe(
      res => {
        if (res.success) {

          this.userChatFriendsWithSettingsOutput = res.result;
          this.friends = res.result.friends;
          // this.selectedUser = res.result.friends[0];
          console.log('getFriends', this.friends);
        }
        else {

        }
      });
  }



  // triggerUnreadMessageCountChangeEvent() {
  //   var totalUnreadMessageCount = 0;

  //   if (this.friends) {
  //     totalUnreadMessageCount = _.reduce(this.friends,
  //       function (memo, friend) {
  //         return memo + friend.unreadMessageCount;
  //       }, 0);
  //   }

  //   abp.event.trigger('app.chat.unreadMessageCountChanged', totalUnreadMessageCount);
  // }

  //#region User


  getShownUserName(tenanycName, userName) {
    return (tenanycName ? tenanycName : '.') + '\\' + userName;
  }

  //#endregion

}
