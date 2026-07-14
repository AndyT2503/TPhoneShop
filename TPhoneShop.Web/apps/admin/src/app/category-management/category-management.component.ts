import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  LucideChevronDown,
  LucideChevronRight,
  LucideFolder,
  LucideFolderOpen,
  LucidePen,
  LucidePlus,
  LucideTrash2,
} from '@lucide/angular';
import {
  ButtonDirective,
  ConfirmPopoverDirective,
  DialogService,
  DialogRef,
} from '@tphone-shop.web/ui';
import { CategoryDto } from '@tphone-shop.web/data-access';
import { CategoryManagementStore } from './store/category-management.store';
import { CategoryDialogComponent } from './ui/category-dialog/category-dialog.component';

@Component({
  selector: 'app-category-management',
  imports: [
    CommonModule,
    ButtonDirective,
    ConfirmPopoverDirective,
    LucidePlus,
    LucideChevronDown,
    LucideChevronRight,
    LucideFolder,
    LucideFolderOpen,
    LucidePen,
    LucideTrash2,
  ],
  templateUrl: './category-management.component.html',
  styleUrl: './category-management.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CategoryManagementComponent implements OnInit {
  private readonly dialog = inject(DialogService);
  readonly store = inject(CategoryManagementStore);

  readonly expandedIds = signal<Set<string>>(new Set());
  private dialogRef?: DialogRef;

  constructor() {
    // Automatically expand all categories when they load initially
    effect(() => {
      const cats = this.store.categories();
      if (cats.length > 0 && this.expandedIds().size === 0) {
        const ids = new Set<string>();
        cats.forEach((c) => ids.add(c.id));
        this.expandedIds.set(ids);
      }
    }, { allowSignalWrites: true });
  }

  ngOnInit(): void {
    this.store.loadCategories(undefined);
  }

  readonly categoryTree = computed(() => {
    const cats = this.store.categories();
    const childrenMap = new Map<string, CategoryDto[]>();
    const roots: CategoryDto[] = [];

    for (const cat of cats) {
      if (!cat.parentId) {
        roots.push(cat);
      } else {
        if (!childrenMap.has(cat.parentId)) {
          childrenMap.set(cat.parentId, []);
        }
        childrenMap.get(cat.parentId)!.push(cat);
      }
    }

    const list: (CategoryDto & {
      level: number;
      hasChildren: boolean;
      isExpanded: boolean;
    })[] = [];

    const traverse = (cat: CategoryDto, level: number) => {
      const children = childrenMap.get(cat.id) || [];
      const isExpanded = this.expandedIds().has(cat.id);

      list.push({
        ...cat,
        level,
        hasChildren: children.length > 0,
        isExpanded,
      });

      if (isExpanded) {
        // Sort children alphabetically
        const sortedChildren = [...children].sort((a, b) => a.name.localeCompare(b.name));
        for (const child of sortedChildren) {
          traverse(child, level + 1);
        }
      }
    };

    // Sort roots alphabetically
    const sortedRoots = [...roots].sort((a, b) => a.name.localeCompare(b.name));
    for (const root of sortedRoots) {
      traverse(root, 0);
    }

    return list;
  });

  toggleExpand(id: string, event: Event): void {
    event.stopPropagation();
    const current = new Set(this.expandedIds());
    if (current.has(id)) {
      current.delete(id);
    } else {
      current.add(id);
    }
    this.expandedIds.set(current);
  }

  openAddCategoryDialog(): void {
    if (this.dialogRef) return;

    this.dialogRef = this.dialog.open(CategoryDialogComponent, {
      title: 'Thêm danh mục',
      data: {
        categories: this.store.categories(),
      },
    });

    this.dialogRef.afterClosed.subscribe(() => {
      this.dialogRef = undefined;
    });
  }

  openEditCategoryDialog(category: CategoryDto): void {
    if (this.dialogRef) return;

    this.dialogRef = this.dialog.open(CategoryDialogComponent, {
      title: 'Cập nhật danh mục',
      data: {
        category,
        categories: this.store.categories(),
      },
    });

    this.dialogRef.afterClosed.subscribe(() => {
      this.dialogRef = undefined;
    });
  }

  deleteCategory(id: string): void {
    this.store.deleteCategory({
      id,
      onSuccess: () => {
        this.store.loadCategories(undefined);
      },
    });
  }
}
