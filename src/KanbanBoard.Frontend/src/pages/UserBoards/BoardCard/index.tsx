import React from 'react';
import { Board } from '../../../models';
import { BoardTitle, SBoardCard } from './styles';

interface IBoardCardProps {
    board: Board;
}

export const BoardCard: React.FC<IBoardCardProps> = ({ board }) => {
    const handleClick = () => {};

    return (
        <SBoardCard onClick={handleClick}>
            <BoardTitle>{board.title}</BoardTitle>
        </SBoardCard>
    );
};
