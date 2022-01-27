import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { BookingDto } from '@shared/models/Quanlykhachsan/booking';
import { BookingService } from '@shared/services/quanlykhachsan/booking.service';
import { DataResponse } from '@shared/models/data/dataresponse';

@Component({
  selector: 'app-main-manage',
  templateUrl: './main-manage.component.html',
  styleUrls: ['./main-manage.component.css'],
  animations: [appModuleAnimation()],
})
export class MainManageComponent implements OnInit {

  hotelId:number;

  constructor(injector: Injector,
    private router: Router,
    private route: ActivatedRoute,
    private bookingService: BookingService) {
    
  }

  ngOnInit(): void {
    this.hotelId = Number(this.route.snapshot.paramMap.get('id'));
  }

}
