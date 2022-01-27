import { Component, Injector, OnInit } from '@angular/core';
import { NewsDto } from '@shared/models/Quanlydothi/tintuc/news';
import { ActivatedRoute, Router } from '@angular/router';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { NewsService } from '@shared/services/quanlydothi/tintuc/news.service';

@Component({
  selector: 'app-update-delete-news',
  templateUrl: './update-delete-news.component.html',
  styleUrls: ['./update-delete-news.component.css'],
  animations: [appModuleAnimation()],
})
export class UpdateDeleteNewsComponent extends AppComponentBase implements OnInit {

  news: NewsDto[];
  selectedNews: NewsDto;

  constructor(injector:Injector, private router: Router,
    private route: ActivatedRoute,
    private newsService: NewsService) {
    super(injector);
  }

  ngOnInit(): void {
    this.getAllNews();
  }

  getAllNews(){
    this.newsService.getAllData('/api/services/app/News/GetAll').subscribe(
      res =>{
        console.log('res', res);
        if (res.success) {
          this.news = res.result.data;
        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
        }
      }
    )
  }

  editNews(news:NewsDto){

  }

  deleteNews(id:number){
    this.newsService.deleteById(id, '/api/services/app/News/Delete?').subscribe(
      res =>{
        console.log('res', res);
        if (res.success) {
          this.messageP.add({ severity: 'success', summary: '', detail: 'Xóa khách sạn thành công!', life: 4000 });
          this.news = this.news.filter(obj => obj.id !== id);
        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
        }
      }
    )
  }
}
