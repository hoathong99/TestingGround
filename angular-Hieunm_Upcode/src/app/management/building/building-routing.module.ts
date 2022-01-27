import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { BuildingComponent } from './building.component';
import { MainBuildingComponent } from './main-building/main-building.component';
import { ManageBuildingComponent } from './main-building/manage-building/manage-building.component';

const routes: Routes = [
  {
    path: '',
    component: BuildingComponent,
    children: [
      { path: '', component: MainBuildingComponent },
      { path: 'main-building', component: MainBuildingComponent },
      { path: 'manage-building', component: ManageBuildingComponent },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class BuildingRoutingModule { }
