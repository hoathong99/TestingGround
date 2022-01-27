import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { HotelComponent } from './hotel.component';
import { ManageHotelComponent } from './manage-hotel/manage-hotel.component';
import { RoomHotelComponent } from './main-manage/room-hotel/room-hotel.component';
import { BookingHotelComponent } from './main-manage/booking-hotel/booking-hotel.component';
import { GuestHotelComponent } from './main-manage/guest-hotel/guest-hotel.component';
import { MainManageComponent } from './main-manage/main-manage.component';

const routes: Routes = [
    {
        path: '',
        component: HotelComponent,
        children: [
            { path: '', component: ManageHotelComponent },
            { path: 'managehotel', component: ManageHotelComponent },
            { path: 'mainmanage/:id', component: MainManageComponent },
            { path: 'roomhotel/:id', component: RoomHotelComponent },
            { path: 'guesthotel/:id', component: GuestHotelComponent },
            { path: 'bookinghotel/:id', component: BookingHotelComponent },
        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class HotelRoutingModule { }
