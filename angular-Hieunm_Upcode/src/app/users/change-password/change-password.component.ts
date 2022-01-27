import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import {
  ChangePasswordDto,
  UserDto,
  UserServiceProxy
} from '@shared/service-proxies/service-proxies';
import { AbpValidationError } from '@shared/components/validation/abp-validation.api';

@Component({
  templateUrl: './change-password.component.html',
  animations: [appModuleAnimation()]
})
export class ChangePasswordComponent extends AppComponentBase implements OnInit {
  saving = false;
  aria_selected = true;
  changePasswordDto = new ChangePasswordDto();
  userDto = new UserDto();
  newPasswordValidationErrors: Partial<AbpValidationError>[] = [
    {
      name: 'pattern',
      localizationKey:
        'PasswordsMustBeAtLeast8CharactersContainLowercaseUppercaseNumber',
    },
  ];
  confirmNewPasswordValidationErrors: Partial<AbpValidationError>[] = [
    {
      name: 'validateEqual',
      localizationKey: 'PasswordsDoNotMatch',
    },
  ];

  constructor(
    injector: Injector,
    private userServiceProxy: UserServiceProxy,
    private router: Router
  ) {
    super(injector);
    this.checknoti = false;
  }
  ngOnInit(): void {
    this.getUserDetail();
    this.aria_selected = true;
    this.checknoti = false;
  }

  changePassword() {
    this.saving = true;

    this.userServiceProxy
      .changePassword(this.changePasswordDto)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe((success) => {
        if (success) {
          abp.message.success('Đổi mật khẩu thành công !', 'Success');
          this.router.navigate(['/']);
        }
      });
  }

  // get user detail
  getUserDetail() {
    this.userServiceProxy
      .getUserDetail()
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe((success) => {
        if (success) {
          this.userDto = success;
          console.log('success', success);
        }
      });

  }

  changeAria() {
    this.aria_selected = this.aria_selected ? false : true;
  }


  //#region Notification

  checknoti = false;

  messager() {
    this.checknoti = true;
  }

  receive(event: boolean) {
    this.checknoti = event;
    console.log('check', this.checknoti);
  }

  //#endregion

}
