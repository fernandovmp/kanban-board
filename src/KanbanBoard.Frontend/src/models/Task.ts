import { User } from './User';

export interface Task {
    id: number;
    summary: string;
    description: string;
    tagColor: string;
    list: number;
    assignments: User[];
}
