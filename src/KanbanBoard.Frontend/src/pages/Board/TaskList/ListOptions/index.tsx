import React, { useContext } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import closeIcon from '../../../../assets/close.svg';
import { BoardContext } from '../../../../contexts';
import { TaskList } from '../../../../models';
import {
    apiDelete,
    isErrorResponse,
} from '../../../../services/kanbanApiService';
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
    const boardContext = useContext(BoardContext);
    const history = useHistory();
    const { boardId } = useParams();

    const handleDelete = async () => {
        onClose();
        const listId = list.id;
        const token = sessionStorage.getItem('jwtToken') ?? '';

        const response = await apiDelete({
            uri: `v1/boards/${boardId}/lists/${listId}`,
            bearerToken: token,
        });
        if (response.data && isErrorResponse(response.data)) {
            if (response.data.status === 401 || response.data.status === 403) {
                history.push('/login');
            }
            return;
        }
        boardContext.setLists(
            boardContext.lists.filter((_list) => _list.id !== listId)
        );
    };

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
                <DeleteButton onClick={handleDelete}>DELETE</DeleteButton>
            </ListOptionsOverlay>
        </RelativeOverlay>
    );
};
