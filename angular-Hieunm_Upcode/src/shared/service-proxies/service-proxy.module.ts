import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from 'abp-ng2-module';

import * as ApiServiceProxies from './service-proxies';
import { ChathubService } from './chathub.service';
import { HomeserverService } from '../services/quanlydancu/homeserver.service';
import { SmarthomeService } from '../services/quanlydancu/smarthome.service';
import { DeviceService } from '../services/quanlydancu/device.service';
import { HotelService } from '../services/quanlykhachsan/hotel.service';
import { BookingService } from '../services/quanlykhachsan/booking.service';
import { RoomService } from '../services/quanlykhachsan/room.service';
import { GuestService } from '../services/quanlykhachsan/guest.service';
import { ProblemSystemService } from '../services/congdongdichvu/problem-system.service';
import { NewsService } from '../services/quanlydothi/tintuc/news.service';

@NgModule({
    providers: [
        ApiServiceProxies.RoleServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.TenantServiceProxy,
        ApiServiceProxies.UserServiceProxy,
        ApiServiceProxies.TokenAuthServiceProxy,
        ApiServiceProxies.AccountServiceProxy,
        ApiServiceProxies.ConfigurationServiceProxy,
        ChathubService,
        HomeserverService,
        SmarthomeService,
        DeviceService,
        HotelService,
        BookingService,
        RoomService,
        GuestService,
        ProblemSystemService,
        NewsService,
        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true }
    ]
})
export class ServiceProxyModule { }
