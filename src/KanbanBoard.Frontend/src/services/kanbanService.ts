import { Board } from '../models';

export interface IKanbanService {
    getBoards(): Board[];
    postBoard(summary: string): void;
}

const boardsData = {
    nextId: 3,
    boards: [
        {
            id: 1,
            summary: 'Project Tasks',
        },
        {
            id: 2,
            summary: 'Template Board',
        },
    ],
};

export const kanbanServiceFactory = (): IKanbanService => ({
    getBoards() {
        return boardsData.boards;
    },
    postBoard(summary: string) {
        boardsData.boards = [
            { id: boardsData.nextId, summary },
            ...boardsData.boards,
        ];
        boardsData.nextId += 1;
    },
});
