import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BuildingComponent } from './building.component';
import { BuildingRoutingModule } from './building-routing.module';
import { MainBuildingComponent } from './main-building/main-building.component';
import {DataViewModule} from 'primeng/dataview';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { ManageBuildingComponent } from './main-building/manage-building/manage-building.component';

@NgModule({
  imports: [
    CommonModule,
    BuildingRoutingModule,
    DataViewModule,
    ButtonModule,
    InputTextModule,
  ],
  declarations: [BuildingComponent, MainBuildingComponent, ManageBuildingComponent]
})
export class BuildingModule { }
