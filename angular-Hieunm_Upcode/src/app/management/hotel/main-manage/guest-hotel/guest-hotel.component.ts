import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { GuestDto } from '@shared/models/Quanlykhachsan/guest';
import { GuestService } from '@shared/services/quanlykhachsan/guest.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-guest-hotel',
  templateUrl: './guest-hotel.component.html',
  styleUrls: ['./guest-hotel.component.css'],
  animations: [appModuleAnimation()],
})
export class GuestHotelComponent extends AppComponentBase implements OnInit {

  hotelId:number;
  guests: GuestDto[] = [];
  selectedGuest:GuestDto;

  constructor(private injector:Injector, 
    private route:ActivatedRoute, 
    private guestService:GuestService)
  {

      super(injector);
      
  }

  ngOnInit(): void {
    this.hotelId = Number(this.route.snapshot.paramMap.get('id'));
    this.getGuestByHotelId();
  }

  getGuestByHotelId(){
    if(this.hotelId){
      this.guestService.getGuestByHotelId(this.hotelId).subscribe(
        res =>{
          console.log('res', res);
          if (res.success) {
            this.guests = res.result.data;
          }
          else {
            this.messageP.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
          }
        }
      )
    }
  };
}
