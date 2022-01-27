import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SchoolComponent } from './school.component';
import { HotelRoutingModule } from './school-routing.module';

@NgModule({
  imports: [
    CommonModule,
    HotelRoutingModule
  ],
  declarations: [SchoolComponent]
})
export class SchoolModule { }
