import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';

export interface Notification {
  id?: number;
  name?: string;
  message?: string;
  type?: boolean;
  creatorId?: string;
  nameCreator?: string;
  creationTime?: Date;
  userId?: string;
  notifiedId?: number;
  isWatched?: boolean;
  isDelete?: boolean;
}

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss'],
  animations: [appModuleAnimation()]
})


export class NotificationComponent extends AppComponentBase implements OnInit {

  notifiedList: Notification[];
  constructor(injector: Injector) {
    super(injector);
  }


  ngOnInit() {
    this.notifiedList = [{ name: 'hieu', message: 'Oke', nameCreator: 'Hieeus' },
    { name: 'hieu', message: 'Oke', nameCreator: 'Hieeus' },
    { name: 'hieu', message: 'Oke', nameCreator: 'Hieeus' }]
  }

  createNoti() {

  }

}
