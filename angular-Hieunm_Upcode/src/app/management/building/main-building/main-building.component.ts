import { Component, OnInit } from '@angular/core';
import { BuildingDto } from '@shared/models/Quanlytoanha/building';

@Component({
  selector: 'app-main-building',
  templateUrl: './main-building.component.html',
  styleUrls: ['../building.component.css', './main-building.component.css']
})
export class MainBuildingComponent implements OnInit {

  buildings: BuildingDto[] = [
    {id:1,name:"Tòa nhà 1",buildingNumber:"BD1",address:"Khu so 1"}, 
    {id:2,name:"Tòa nhà 2",buildingNumber:"BD2",address:"Khu so 1"}, 
    {id:2,name:"Tòa nhà 3",buildingNumber:"BD3",address:"Khu so 1"}, 
    {id:2,name:"Tòa nhà 4",buildingNumber:"BD4",address:"Khu so 1"}
  ];

  constructor() { }

  ngOnInit(): void {

  }

}
