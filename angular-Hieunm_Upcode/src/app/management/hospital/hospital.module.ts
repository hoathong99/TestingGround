import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HospitalComponent } from './hospital.component';
import { HospitalRoutingModule } from './hospital-routing.module';

@NgModule({
  imports: [
    CommonModule,
    HospitalRoutingModule
  ],
  declarations: [HospitalComponent]
})
export class HospitalModule { }
