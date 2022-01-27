import { Injector } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { NewsTypeDto } from '@shared/models/Quanlydothi/tintuc/news-type';
import { AppConsts } from '@shared/AppConsts';
import { NewsService } from '@shared/services/quanlydothi/tintuc/news.service';
import { NewsDto } from '@shared/models/Quanlydothi/tintuc/news';

@Component({
  selector: 'app-add-news',
  templateUrl: './add-news.component.html',
  styleUrls: ['./add-news.component.css'],
  animations: [appModuleAnimation()],
})
export class AddNewsComponent extends AppComponentBase implements OnInit {

  news: NewsDto = {id: 0,title:'', content:'', datePost:new Date(), poster:'', newsTypeId: 0};
  newsType: NewsTypeDto[];
  selectedNewsType: NewsTypeDto;

  constructor(injector:Injector, private router: Router, private newsService:NewsService) {
    super(injector);

  }

  ngOnInit(): void {
    this.newsType = AppConsts.newsType;
  }

  uploadNews(){
    if(this.news.content != ''){

      this.news.poster = 'admin';
      this.news.newsTypeId = this.selectedNewsType.id;

      this.newsService.createNews(this.news).subscribe(
        res => {
          if (res.success) {
            this.messageP.add({ severity: 'success', summary: '', detail: 'Đăng tin thành công!', life: 4000 });
          }
          else {
            this.messageP.add({ severity: 'error', summary: '', detail: 'Thêm thất bại !', life: 4000 });
          }
        }
      )
    }
  }
}
