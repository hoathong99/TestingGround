import { Component, Injector, OnInit, DoCheck, OnChanges, SimpleChanges } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { DataResponse } from '@shared/models/data/dataresponse';
import { RoomDto } from '@shared/models/Quanlykhachsan/room';
import { RoomService } from '@shared/services/quanlykhachsan/room.service';
import { BehaviorSubject, Observer, Observable, of } from 'rxjs';

@Component({
    selector: 'app-room-hotel',
    templateUrl: './room-hotel.component.html',
    styleUrls: ['./room-hotel.component.css'],
    animations: [appModuleAnimation()],
  })
  export class RoomHotelComponent extends AppComponentBase implements OnChanges, DoCheck, OnInit {

    hotelId: number;
    rooms = new Observable<RoomDto[]>();

    constructor(
        injector: Injector,
        private router: Router,
        private route: ActivatedRoute,
        private roomService: RoomService
      ) {
        super(injector);
        
      };

    ngOnInit():void{
      this.hotelId = Number(this.route.snapshot.paramMap.get('id'));      
      this.rooms = this.roomService.GetRoomByHotelId(this.hotelId);
    };

    ngDoCheck(){

    };
    ngOnChanges(changes: SimpleChanges){

    };

    closeRoomHotel(){

    };

    submitRoomHotel(){

    };
    openEditRoom(){

    }
  }