import { ChangeDetectionStrategy, Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Router } from '@angular/router';
import { AppComponentBase } from '@shared/app-component-base';
import { HotelService } from '@shared/services/quanlykhachsan/hotel.service';
import { DataResponse } from '@shared/models/data/dataresponse';
import { HotelDto } from '@shared/models/Quanlykhachsan/hotel';

@Component({
    selector: 'app-manage-hotel',
    templateUrl: './manage-hotel.component.html',
    styleUrls: ['./manage-hotel.component.css'],
    animations: [appModuleAnimation()],
  })
  export class ManageHotelComponent extends AppComponentBase implements OnInit {

    constructor(injector: Injector, private router: Router, private hotelService:HotelService) {
      super(injector);
    };
  
    hotels:HotelDto[] = [];
    totalRecords: number = this.hotels.length;
    selectedHotel:[];
    stateHotel: number = 0;
    stateNewHotel: boolean = false;
    stateEditHotel: boolean = false;
    createHotel: HotelDto;
    hotelRes: HotelDto;
    dataResponse: DataResponse;
    loading: boolean = false;
    // sortOptions: SelectItem[];
    sortOrder: number;
    sortField: string;
    first = 0;
    rows = 10;
  
    ngOnInit(): void {
      this.loading = true;
      this.refresh();
      this.loading = false;
  
    }
  
    refresh(): void {
      this.loading = true;
      this.getDataPage();
      this.loading = false;
    }
  
    public getDataPage(): void {
      this.getListHotel();
    }
  
    protected getListHotel() {
      const url = "/api/services/app/Hotel/GetHotels";
      return this.hotelService.getAllData(url).subscribe(
        res => {
          this.dataResponse = res.result as any;
          console.log('res', res);
          if (res.success) {
            this.hotels = res.result.data;
          }
          else {
            this.messageP.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
          }
        }
      )
  
    }
  
  
    editHotel(hotel: HotelDto) {
      
      this.stateEditHotel = true;
      this.createHotel = JSON.parse(JSON.stringify(hotel));
    }
  
    addHotel() {
      this.stateNewHotel = true;
      this.createHotel = {};
    }
  
    closeHotel() {
      this.stateNewHotel = false;
      this.stateEditHotel = false;
      console.log('create', this.createHotel);
    }
  
    submitHotel() {
      console.log('create', this.createHotel);
      if(this.stateNewHotel){
        this.hotelService.createHotel(this.createHotel).subscribe(
          res => {
            if (res.success) {
              if (res.result) {
                this.hotels = JSON.parse(JSON.stringify(res.result.data));    
              }   
              this.messageP.add({ severity: 'success', summary: '', detail: 'Thao tác thành công!', life: 4000 });
            }
            else {
              this.messageP.add({ severity: 'error', summary: '', detail: 'Thao tác thất bại !', life: 4000 });
            }
          }
        );
      }
      else if(this.stateEditHotel){
        this.hotelService.updateHotel(this.createHotel).subscribe(
          res => {
            if (res.success) {
              if (res.result) {
                this.hotels = JSON.parse(JSON.stringify(res.result.data));   
              }   
              this.messageP.add({ severity: 'success', summary: '', detail: 'Thao tác thành công!', life: 4000 });
            }
            else {
              this.messageP.add({ severity: 'error', summary: '', detail: 'Thao tác thất bại !', life: 4000 });
            }
          }
        );
      }
      
      this.stateNewHotel = false;
      this.stateEditHotel = false;
    }
  
  
    deleteHotel(id: number) {
      const url = "/api/services/app/Hotel/DeleteHotel?id=";
      this.hotelService.deleteById(id, url).subscribe(
        res => {
          if (res.success) {
            this.messageP.add({ severity: 'success', summary: '', detail: 'Xóa khách sạn thành công!', life: 4000 });
            this.hotels = this.hotels.filter(obj => obj.id !== id);
          }
          else {
            this.messageP.add({ severity: 'error', summary: '', detail: 'Xóa thất bại !', life: 4000 });
          }
        }
      );
    }
  
    /**
     * Phân trang
     */
    next() {
      this.first = this.first + this.rows;
    }
  
    prev() {
        this.first = this.first - this.rows;
    }
  
    reset() {
        this.first = 0;
    }
  
    isLastPage(): boolean {
        return this.hotels ? this.first === (this.hotels.length - this.rows): true;
    }
  
    isFirstPage(): boolean {
        return this.hotels ? this.first === 0 : true;
    }
  }