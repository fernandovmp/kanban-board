import React from 'react';
import closeIcon from '../../../../assets/close.svg';
import { TaskList } from '../../../../models';
import {
    CloseButton,
    DeleteButton,
    Header,
    ListOptionsOverlay,
    RelativeOverlay,
} from './styles';

interface IListOptionsProps {
    list: TaskList;
    onClose: () => void;
}

export const ListOptions: React.FC<IListOptionsProps> = ({ list, onClose }) => {
    return (
        <RelativeOverlay>
            <ListOptionsOverlay
                title={
                    <Header>
                        List options
                        <CloseButton src={closeIcon} onClick={onClose} />
                    </Header>
                }
                onClose={onClose}
            >
                <DeleteButton>DELETE</DeleteButton>
            </ListOptionsOverlay>
        </RelativeOverlay>
    );
};
