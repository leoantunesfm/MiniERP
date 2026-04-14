import { Directive, Input, TemplateRef, ViewContainerRef, inject } from '@angular/core';
import { AuthorizationService } from '../services/authorization';

@Directive({
  selector: '[appHasRole]',
  standalone: true
})
export class HasRoleDirective {
  private templateRef = inject(TemplateRef<any>);
  private viewContainer = inject(ViewContainerRef);
  private authZService = inject(AuthorizationService);
  private isVisible = false;

  @Input() set appHasRole(roles: string[]) {
    const hasAccess = this.authZService.hasRole(roles);

    if (hasAccess && !this.isVisible) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.isVisible = true;
    } else if (!hasAccess && this.isVisible) {
      this.viewContainer.clear();
      this.isVisible = false;
    }
  }
}