import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { Inject, InjectionToken, Injector, Optional } from '@angular/core';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { DataResponse } from '../models/data/dataresponse';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';

import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export abstract class BaseService {

    http: HttpClient;
    baseUrl: string;
    urlApi = 'http://localhost:21021';
    constructor(

        injector: Injector,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string,

    ) {
        this.http = injector.get(HttpClient);
        this.baseUrl = baseUrl ? baseUrl : "";

    }

    public getAllData(url: string): Observable<any> {
        return this.http.get<DataResponse>(this.urlApi + url).pipe(
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }


    // getRoles(): Observable<any> {
    //     let url_ = this.baseUrl + "/api/services/app/User/GetRoles";
    //     url_ = url_.replace(/[?&]$/, "");

    //     let options_: any = {
    //         observe: "response",
    //         responseType: "blob",
    //         headers: new HttpHeaders({
    //             "Accept": "text/plain"
    //         })
    //     };

    //     return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
    //         return this.processGetRoles(response_);
    //     })).pipe(_observableCatch((response_: any) => {
    //         if (response_ instanceof HttpResponseBase) {
    //             try {
    //                 return this.processGetRoles(<any>response_);
    //             } catch (e) {
    //                 return <Observable<any>><any>_observableThrow(e);
    //             }
    //         } else
    //             return <Observable<any>><any>_observableThrow(response_);
    //     }));
    // }

    // protected processGetRoles(response: HttpResponseBase): Observable<any> {
    //     const status = response.status;
    //     const responseBlob =
    //         response instanceof HttpResponse ? response.body :
    //             (<any>response).error instanceof Blob ? (<any>response).error : undefined;

    //     let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
    //     if (status === 200) {
    //         return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
    //             let result200: any = null;
    //             let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
    //             result200 = any.fromJS(resultData200);
    //             return _observableOf(result200);
    //         }));
    //     } else if (status !== 200 && status !== 204) {
    //         return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
    //             return throwException("An unexpected server error occurred.", status, _responseText, _headers);
    //         }));
    //     }
    //     return _observableOf<RoleDtoListResultDto>(<any>null);
    // }

    public getById(id: number, url: string): Observable<any> {
        return this.http.get<DataResponse>(this.urlApi + url + `?id=${id}`).pipe(
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }

    public getByCode(code: string, url: string): Observable<any> {
        return this.http.get<DataResponse>(this.urlApi + url + `?code=${code}`).pipe(
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }

    public search(text: string, url: string): Observable<any> {
        var data = { data: text }
        return this.http.post(this.urlApi + url, data).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }

    public deleteById(id: number, url: string): Observable<DataResponse> {

        return this.http.delete(this.urlApi + url + `${id}`).pipe(
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

function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader();
            reader.onload = event => {
                observer.next((<any>event.target).result);
                observer.complete();
            };
            reader.readAsText(blob);
        }
    });
}