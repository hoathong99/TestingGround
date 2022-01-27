import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HotelComponent } from './hotel.component';
import {FormsModule} from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { InputTextModule } from 'primeng/inputtext';
import { ToolbarModule } from 'primeng/toolbar';
import { TableModule } from 'primeng/table';
import {DataViewModule} from 'primeng/dataview';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import {PanelModule} from 'primeng/panel';
import {DropdownModule} from 'primeng/dropdown';
import {RatingModule} from 'primeng/rating';
import {RippleModule} from 'primeng/ripple';
import { FileUploadModule } from 'primeng/fileupload';
import { HotelRoutingModule } from './hotel-routing.module';
import { ManageHotelComponent } from './manage-hotel/manage-hotel.component';
import { RoomHotelComponent } from './main-manage/room-hotel/room-hotel.component';
import { BookingHotelComponent } from './main-manage/booking-hotel/booking-hotel.component';
import { MainManageComponent } from './main-manage/main-manage.component';
import { GuestHotelComponent } from './main-manage/guest-hotel/guest-hotel.component';

@NgModule({
  imports: [
    CommonModule,
    HotelRoutingModule,
    FormsModule,
    RouterModule,
    DialogModule,
    ToastModule,
    InputTextModule,
    TableModule,
    DataViewModule,
    ToolbarModule,
    ButtonModule,
    PanelModule,
    DropdownModule,
    RatingModule,
    RippleModule,
    FileUploadModule,
  ],
  declarations: [
    HotelComponent,
    ManageHotelComponent,
    RoomHotelComponent,
    BookingHotelComponent,
    MainManageComponent,
    GuestHotelComponent,
  ],
  bootstrap: [ HotelComponent ],
})
export class HotelModule { }
