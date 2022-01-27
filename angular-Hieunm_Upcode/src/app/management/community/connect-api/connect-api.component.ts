import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { DeviceDto } from '@shared/models/Quanlydancu/smarthome/device';
import { DeviceAPIDto } from '@shared/models/Quanlydancu/smarthome/deviceapi';
import { HomeServerDto } from '@shared/models/Quanlydancu/smarthome/homeserver';
import { SmartHomeDto } from '@shared/models/Quanlydancu/smarthome/smarthome';
import { DeviceService } from '@shared/services/quanlydancu/device.service';
import { HomeserverService } from '@shared/services/quanlydancu/homeserver.service';
import { SmarthomeService } from '@shared/services/quanlydancu/smarthome.service';
import { SelectItem } from 'primeng/api';

@Component({
  selector: 'app-connect-api',
  templateUrl: './connect-api.component.html',
  styleUrls: ['./connect-api.component.css'],
  animations: [appModuleAnimation()]
})
export class ConnectApiComponent extends AppComponentBase implements OnInit {

  deviceapis: DeviceAPIDto[] = [];
  devices: SelectItem[] = [];
  homeservers: SelectItem[] = [];
  deviceapi: DeviceAPIDto;

  dialogCreateDeivceApi: boolean = false;
  checkUpdate: boolean = false;
  createDeviceApi: DeviceAPIDto = {};
  deviceApiResponse: DeviceAPIDto;
  searchText: string;

  smartHome: SmartHomeDto;
  statuses: SelectItem[];
  constructor(
    injector: Injector,
    private router: Router,
    private deviceService: DeviceService,
    private homeserverService: HomeserverService,
    private smarthomeService: SmarthomeService
  ) {
    super(injector);
    this.statuses = [
      { label: 'KNX', value: 'KNX' },
      { label: 'ZigBee', value: 'ZigBee' },
      { label: 'Z-Wave', value: 'Z-Wave' },
    ];
  }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.getListDeviceAPI();
    this.getListDevices();
    this.getListHomeServer();
  }

  protected getListDeviceAPI() {
    const url = "/api/services/app/Device/GetAllDeviceAPI";
    return this.deviceService.getAllData(url).subscribe(
      res => {
        if (res.success) {
          this.deviceapis = res.result.data;
          console.log('deviceapis', this.deviceapis);
        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
        }
      }
    )

  }

  protected getListDevices() {
    const url = "/api/services/app/Device/GetAllDevice";
    return this.deviceService.getAllData(url).subscribe(
      res => {
        if (res.success) {
          this.devices = res.result.data.map(val => ({
            value: val.id,
            label: val.name
          }));
          console.log('device', this.devices);
        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
        }
      }
    )

  }

  protected getListHomeServer() {
    const url = "/api/services/app/HomeServer/GetAllHomeServer";
    return this.homeserverService.getAllData(url).subscribe(
      res => {
        if (res.success) {
          this.homeservers = res.result.data.map(val => ({
            value: val.id,
            label: val.name
          }));
          console.log('homeservers', this.homeservers);
        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
        }
      }
    )

  }

  //#region API
  updateDevice(item: DeviceDto) {
    this.dialogCreateDeivceApi = true;
    this.createDeviceApi = item;
    this.checkUpdate = true;
  }


  openCreateDeviceAPI() {
    this.dialogCreateDeivceApi = true;
    this.createDeviceApi = {};
  }

  closeCreateDeviceAPI() {
    this.dialogCreateDeivceApi = false;
    this.createDeviceApi = {};
  }

  submitCreateDeviceAPI() {
    this.createDeviceApi.deviceName = this.devices.find(x => x.value == this.createDeviceApi.deviceId).label;
    this.createDeviceApi.homeServerName = this.homeservers.find(x => x.value == this.createDeviceApi.homeServerId).label;
    this.deviceService.createOrUpdateDeviceApi(this.createDeviceApi).subscribe(
      res => {
        if (res.success) {
          this.deviceApiResponse = res.result;
          if (!this.checkUpdate) {
            this.deviceapis.unshift(this.deviceApiResponse);
            console.log('deviceApiResponse', this.deviceApiResponse);
            this.messageP.add({ severity: 'success', summary: '', detail: 'Tạo mới api thành công!', life: 4000 });
          } else {
            this.messageP.add({ severity: 'success', summary: '', detail: 'Sửa api thành công!', life: 4000 });
          }

        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Có lỗi xảy ra !', life: 4000 });
        }
        this.checkUpdate = false;
      });
    this.dialogCreateDeivceApi = false;
  }

  deleteDeviceAPI(id: number) {
    const url = "/api/services/app/Device/DeleteDeviceAPI?id=";
    this.deviceService.deleteById(id, url).subscribe(
      res => {
        if (res.success) {
          this.messageP.add({ severity: 'success', summary: '', detail: 'Xóa thiết bị thành công!', life: 4000 });
          this.deviceapis = this.deviceapis.filter(obj => obj.id !== id);
        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Xóa thất bại !', life: 4000 });
        }
      }
    );

  }
  //#endregion

  //#region Nhà thông minh
  search() {
    console.log('editSmartHome', this.searchText);
    this.smarthomeService.search(this.searchText, '/api/services/app/SmartHome/SearchSmartHome').subscribe(
      res => {
        if (res.success) {
          this.smartHome = res.result.data;
          console.log('editSmartHome', res);
        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
        }
      });
  }
  //#endregion

}
