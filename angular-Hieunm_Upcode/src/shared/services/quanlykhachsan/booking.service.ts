import { Injectable, InjectionToken, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { BaseService } from '../base.service';
import { BookingDto } from '@shared/models/Quanlykhachsan/booking';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class BookingService extends BaseService {

    constructor(
        injector: Injector,
    ) {
        super(injector);
    }


    public createHotel(data: BookingDto): Observable<any> {
        return this.http.post(this.urlApi + "/api/services/app/Booking/CreateBooking", data).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }

    public getBookingByHotelId(hotelId:number): Observable<any> {
        return this.http.get(this.urlApi + "/api/services/app/Booking/GetBookingByHotelId?hotelId="+hotelId ).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }
    
}