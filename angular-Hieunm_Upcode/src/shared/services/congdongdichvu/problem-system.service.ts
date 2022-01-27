import { Injectable, InjectionToken, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ProblemSystemDto } from '../../models/Quanlydancu/dichvu/problem-system';
import { BaseService } from '../base.service';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class ProblemSystemService extends BaseService {

    constructor(
        injector: Injector,
    ) {
        super(injector);
    }

    public createOrUpdateProblem(data: ProblemSystemDto): Observable<any> {
        return this.http.post(this.urlApi + "/api/services/app/ProblemSystem/CreateOrUpdateProblem", data).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }


    //#region API device
    // public createOrUpdateDeviceApi(data: ProblemSystemDto): Observable<any> {
    //     return this.http.post(this.urlApi + "/api/services/app/ProblemSystem/CreateOrUpdateDeviceAPI", data).pipe(
    //         map((res: any) => {
    //             return res;
    //         }),
    //         catchError(err => {
    //             console.log("error ", err);
    //             return null;
    //         })
    //     );
    // }
    //#endregion
}
