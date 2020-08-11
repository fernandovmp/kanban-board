import { Board } from '../models';

export interface IKanbanService {
    getBoards(): Board[];
    postBoard(summary: string): void;
    getBoard(id: number): Board | undefined;
}

const boardsData = {
    nextId: 3,
    boards: [
        {
            id: 1,
            title: 'Project Tasks',
            lists: [
                {
                    id: 1,
                    title: 'To Do',
                    tasks: [
                        {
                            id: 1,
                            summary: 'Task #1',
                            tagColor: '9932CC',
                        },
                        {
                            id: 2,
                            summary: 'Task #2',
                            tagColor: '9932CC',
                        },
                        {
                            id: 3,
                            summary: 'Task #3',
                            tagColor: '9932CC',
                        },
                    ],
                },
                {
                    id: 2,
                    title: 'Doing',
                    tasks: [
                        {
                            id: 4,
                            summary:
                                'Task #4 Progress: 90% Update docs: Add project description in README and links to definitions in docs directory',
                            tagColor: 'FFEA31',
                        },
                    ],
                },
            ],
        },
        {
            id: 2,
            title: 'Template Board',
        },
    ],
};

export const kanbanServiceFactory = (): IKanbanService => ({
    getBoards() {
        return boardsData.boards;
    },
    postBoard(title: string) {
        boardsData.boards = [
            { id: boardsData.nextId, title },
            ...boardsData.boards,
        ];
        boardsData.nextId += 1;
    },
    getBoard(id: number) {
        return boardsData.boards.find((board) => board.id === id);
    },
});
