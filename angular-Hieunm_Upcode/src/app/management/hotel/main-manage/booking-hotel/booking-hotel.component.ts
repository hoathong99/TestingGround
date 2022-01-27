import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { BookingDto } from '@shared/models/Quanlykhachsan/booking';
import { BookingService } from '@shared/services/quanlykhachsan/booking.service';
import { DataResponse } from '@shared/models/data/dataresponse';

@Component({
    selector: 'app-booking-hotel',
    templateUrl: './booking-hotel.component.html',
    styleUrls: ['./booking-hotel.component.css'],
    animations: [appModuleAnimation()],
  })
  export class BookingHotelComponent extends AppComponentBase implements OnInit {

    hotelId: number;
    bookings: BookingDto[] = [];
    selectedBooking:[];
    dataResponse: DataResponse;

    constructor(
        injector: Injector,
        private router: Router,
        private route: ActivatedRoute,
        private bookingService: BookingService
      ) {
        super(injector);
        
      };

    ngOnInit():void{
      this.hotelId = Number(this.route.snapshot.paramMap.get('id'));
      this.getBookingByHotelId();
    };

    getBookingByHotelId(){
      if(this.hotelId){
        this.bookingService.getBookingByHotelId(this.hotelId).subscribe(
          res =>{
            this.dataResponse = res.result as any;
            console.log('res', res);
            if (res.success) {
              this.bookings = res.result.data;
            }
            else {
              this.messageP.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
            }
          }
        )
      }
    };
  }