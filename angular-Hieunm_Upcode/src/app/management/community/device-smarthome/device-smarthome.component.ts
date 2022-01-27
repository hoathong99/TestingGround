import { Component, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DeviceDto } from '@shared/models/Quanlydancu/smarthome/device';
import { DeviceService } from '@shared/services/quanlydancu/device.service';
import { HomeServerDto } from '@shared/models/Quanlydancu/smarthome/homeserver';
import { MessageService, SelectItem } from 'primeng/api';
import { HomeserverService } from '@shared/services/quanlydancu/homeserver.service';

@Component({
  selector: 'app-device-smarthome',
  templateUrl: './device-smarthome.component.html',
  styleUrls: ['./device-smarthome.component.css'],
  animations: [appModuleAnimation()]
})
export class DeviceSmarthomeComponent implements OnInit {


  devices: DeviceDto[] = [];
  homeservers: HomeServerDto[] = [];

  createDevice: DeviceDto = {};
  deviceResponse: DeviceDto;

  createHomeserver: HomeServerDto = {};
  homeserverResponse: HomeServerDto;

  statuses: SelectItem[];
  deviceoptions: SelectItem[];
  dialogCreateDeivce: boolean = false;
  dialogCreateHomeserver: boolean = false;
  checkUpdate: boolean = false;

  constructor(
    private deviceService: DeviceService,
    private message: MessageService,
    private homeserverService: HomeserverService
  ) { }

  ngOnInit() {
    this.loadData();
    this.statuses = [
      { label: 'Raspberry Pi', value: 'Raspberry Pi' },
      { label: 'Somfy', value: 'Somfy' },
      { label: 'Iridium', value: 'Iridium' },
    ];
    this.deviceoptions = [
      { label: 'Smart Lighting', value: 'Smart Lighting' },
      { label: 'Smart Air', value: 'Smart Air' },
      { label: 'Smart Water', value: 'Smart Water' },
      { label: 'Smart Connection', value: 'Smart Connection' },
      { label: 'Smart Fire Alarm', value: 'Smart Fire Alarm' },
      { label: 'Smart Door Entry', value: 'Smart Door Entry' },
      { label: 'Smart Security', value: 'Smart Security' },
      { label: 'Smart Curtain', value: 'Smart Curtain' }
    ];
  }

  loadData() {
    this.getListDevices();
    this.getListHomeServer();
  }

  protected getListDevices() {
    const url = "/api/services/app/Device/GetAllDevice";
    return this.deviceService.getAllData(url).subscribe(
      res => {
        if (res.success) {
          this.devices = res.result.data;
          console.log('device', this.devices);
        }
        else {
          this.message.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
        }
      }
    )

  }

  protected getListHomeServer() {
    const url = "/api/services/app/HomeServer/GetAllHomeServer";
    return this.homeserverService.getAllData(url).subscribe(
      res => {
        if (res.success) {
          this.homeservers = res.result.data;
          console.log('homeservers', this.homeservers);
        }
        else {
          this.message.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
        }
      }
    )

  }

  //#region Device

  updateDevice(item: DeviceDto) {
    this.dialogCreateDeivce = true;
    this.createDevice = item;
    this.checkUpdate = true;
  }

  openCreateDevice() {
    this.dialogCreateDeivce = true;
    this.createDevice = {};
  }

  closeCreateDevice() {
    this.dialogCreateDeivce = false;
    this.createDevice = {};
  }

  submitCreateDevice() {

    this.deviceService.createOrUpdateDevice(this.createDevice).subscribe(
      res => {
        if (res.success) {
          this.deviceResponse = res.result;

          if (this.checkUpdate) {
            this.message.add({ severity: 'success', summary: '', detail: 'Sửa thiết bị thành công!', life: 4000 });
          }
          else {
            this.devices.unshift(this.deviceResponse);
            console.log('create update', this.createDevice, this.checkUpdate);
            this.message.add({ severity: 'success', summary: '', detail: 'Tạo mới thiết bị thành công!', life: 4000 });
          }

          this.createDevice = {};

        }
        else {
          this.message.add({ severity: 'error', summary: '', detail: 'Có lỗi! Xin hãy liên hệ quản trị viên!', life: 4000 });
        }
        this.checkUpdate = false;
      });
    this.dialogCreateDeivce = false;
  }

  deleteDevice(id: number) {
    const url = "/api/services/app/Device/DeleteDevice?id=";
    this.deviceService.deleteById(id, url).subscribe(
      res => {
        if (res.success) {
          this.message.add({ severity: 'success', summary: '', detail: 'Xóa thiết bị thành công!', life: 4000 });
          this.devices = this.devices.filter(obj => obj.id !== id);
        }
        else {
          this.message.add({ severity: 'error', summary: '', detail: 'Xóa thất bại !', life: 4000 });
        }
      }
    );

  }

  //#endregion

  //#region homeServer

  updateHomeServer(item: HomeServerDto) {
    this.dialogCreateHomeserver = true;
    this.createHomeserver = item;
    this.checkUpdate = true;

  }
  openCreateHomeServer() {
    this.dialogCreateHomeserver = true;
    this.createHomeserver = {};
  }

  closeCreateHomeServer() {
    this.dialogCreateHomeserver = false;
    this.createHomeserver = {};
  }

  submitCreateHomeServer() {
    this.homeserverService.createOrUpdateHomeServer(this.createHomeserver).subscribe(
      res => {
        if (res.success) {
          this.homeserverResponse = res.result;
          if (this.checkUpdate) {
            this.message.add({ severity: 'success', summary: '', detail: 'Sửa home server thành công!', life: 4000 });
          }
          else {

            this.homeservers.unshift(this.homeserverResponse);
            this.message.add({ severity: 'success', summary: '', detail: 'Tạo mới home server thành công!', life: 4000 });
          }

          this.createHomeserver = {};

        }
        else {
          this.message.add({ severity: 'error', summary: '', detail: 'Có lỗi xảy ra! Xin hãy liên hệ quản trị viên !', life: 4000 });
        }
        this.checkUpdate = false;
      });
    this.dialogCreateHomeserver = false;
  }

  deleteHomeServer(id: number) {
    const url = "/api/services/app/HomeServer/DeleteHomeServer?id=";
    this.homeserverService.deleteById(id, url).subscribe(
      res => {
        if (res.success) {

          this.devices = this.devices.filter(obj => obj.id !== id);
          console.log('create update', this.createDevice, this.checkUpdate);
          this.message.add({ severity: 'success', summary: '', detail: 'Xóa home server thành công!', life: 4000 });
        }
        else {
          this.message.add({ severity: 'error', summary: '', detail: 'Xóa home server thất bại !', life: 4000 });
        }
      }
    );

  }
  //#endregion

}
