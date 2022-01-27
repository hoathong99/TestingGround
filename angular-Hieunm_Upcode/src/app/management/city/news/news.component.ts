import { Injector } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css'],
  animations: [appModuleAnimation()],
})
export class NewsComponent extends AppComponentBase implements OnInit {

  constructor(injector:Injector, private router: Router, private route: ActivatedRoute) {
    super(injector);
   }

  ngOnInit() {
  }

}
