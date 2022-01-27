import { Injectable, InjectionToken, Injector } from '@angular/core';
import { HomeServerDto } from '@shared/models/Quanlydancu/smarthome/homeserver';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { BaseService } from '../base.service';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class HomeserverService extends BaseService {


  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  public createOrUpdateHomeServer(data: HomeServerDto): Observable<any> {
    return this.http.post(this.urlApi + "/api/services/app/HomeServer/CreateOrUpdateHomeServer", data).pipe(
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
