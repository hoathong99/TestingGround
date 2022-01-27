import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-community-service',
  templateUrl: './community-service.component.html',
  styleUrls: ['./community-service.component.css'],
  animations: [appModuleAnimation()],
})
export class CommunityServiceComponent extends AppComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit() {
  }

}
