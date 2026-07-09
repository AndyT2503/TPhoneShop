import {
  ChangeDetectionStrategy,
  Component,
  ViewEncapsulation,
  computed,
  input,
  output,
} from '@angular/core';
import { LucideArrowLeft, LucideArrowRight } from '@lucide/angular';

@Component({
  standalone: true,
  selector: 'lib-table',
  imports: [LucideArrowLeft, LucideArrowRight],
  templateUrl: './table.component.html',
  styleUrl: './table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
})
export class TableComponent {
  readonly showPagination = input(false);
  readonly totalCount = input<number>(0);
  readonly pageNumber = input<number>(1);
  readonly pageSize = input<number>(10);
  // eslint-disable-next-line @angular-eslint/no-output-on-prefix
  readonly onPageNumberChange = output<number>();

  readonly totalPages = computed(() =>
    Math.max(1, Math.ceil(this.totalCount() / this.pageSize())),
  );

  readonly paginationPages = computed(() => {
    const total = this.totalPages();
    const current = this.pageNumber();

    if (total <= 5) {
      return Array.from({ length: total }, (_, index) => index + 1 as number | 'ellipsis');
    }

    const pages: Array<number | 'ellipsis'> = [1];

    if (current > 3) {
      pages.push('ellipsis');
    }

    const start = Math.max(2, current - 1);
    const end = Math.min(total - 1, current + 1);

    for (let page = start; page <= end; page += 1) {
      pages.push(page);
    }

    if (current < total - 2) {
      pages.push('ellipsis');
    }

    pages.push(total);
    return pages;
  });

  changePage(page: number): void {
    const normalizedPage = Math.min(this.totalPages(), Math.max(1, page));

    if (normalizedPage !== this.pageNumber()) {
      this.onPageNumberChange.emit(normalizedPage);
    }
  }
}
