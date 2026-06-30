


export interface ActionMeta {
  label: string;
  modifier: string;
}


export const ACTION_META: Record<string, ActionMeta> = {
  read: {
    label: 'Xem',
    modifier: 'read',
  },
  create: {
    label: 'Tạo mới',
    modifier: 'create',
  },
  update: {
    label: 'Cập nhật',
    modifier: 'update',
  },
  delete: {
    label: 'Xóa',
    modifier: 'delete',
  },
};

