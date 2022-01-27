import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { ProblemSystemDto } from '@shared/models/Quanlydancu/dichvu/problem-system';
import { ProblemSystemService } from '@shared/services/congdongdichvu/problem-system.service';

@Component({
  selector: 'app-problem-system',
  templateUrl: './problem-system.component.html',
  styleUrls: ['./problem-system.component.css'],
  animations: [appModuleAnimation()]
})
export class ProblemSystemComponent extends AppComponentBase implements OnInit {

  problems: ProblemSystemDto[] = [];

  problemCreateorEdit: ProblemSystemDto;

  dialogcreate: boolean;

  constructor(injector: Injector,
    problemService: ProblemSystemService
  ) {
    super(injector);
  }

  ngOnInit() {
  }

}
