import { Directive, HostBinding } from '@angular/core';

@Directive({
  selector: '[libTbCenter]',
  standalone: true,
})
export class TbCenterDirective {
  @HostBinding('style.textAlign') readonly textAlign = 'center';
}

@Directive({
  selector: '[libTbLeft]',
  standalone: true,
})
export class TbLeftDirective {
  @HostBinding('style.textAlign') readonly textAlign = 'left';
}

@Directive({
  selector: '[libTbRight]',
  standalone: true,
})
export class TbRightDirective {
  @HostBinding('style.textAlign') readonly textAlign = 'right';
}
