import React, { useEffect, useState } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import {
    apiPatch,
    isErrorResponse,
} from '../../../../services/kanbanApiService';
import { getJwtToken } from '../../../../services/tokenService';
import {
    ButtonsWrapper,
    CancelButton,
    DescriptionCard,
    DescriptionTextArea,
    SaveButton,
} from './styles';

interface IEditableDescription {
    taskDescription: string;
}

export const EditableDescription: React.FC<IEditableDescription> = ({
    taskDescription,
}) => {
    const [description, setDescription] = useState('');
    const [newDescription, setNewDescription] = useState('');
    const [isEditing, setIsEditing] = useState(false);
    const history = useHistory();
    const { boardId, taskId } = useParams();

    useEffect(() => setDescription(taskDescription), [taskDescription]);

    const handleBeginEdit = () => {
        setNewDescription(description);
        setIsEditing(true);
    };

    const handleSave = async () => {
        setIsEditing(false);
        const token = getJwtToken();
        const response = await apiPatch({
            uri: `v1/boards/${boardId}/tasks/${taskId}`,
            body: {
                description: newDescription,
            },
            bearerToken: token,
        });
        if (response.data && isErrorResponse(response.data)) {
            if (response.data.status === 401 || response.data.status === 403) {
                history.push('/login');
            }
            return;
        }
        setDescription(newDescription);
    };

    return (
        <>
            {isEditing ? (
                <>
                    <DescriptionTextArea
                        value={newDescription}
                        onChange={(e) => setNewDescription(e.target.value)}
                        minRows={5}
                        autoFocus
                    />
                    <ButtonsWrapper>
                        <CancelButton onClick={() => setIsEditing(false)}>
                            Cancel
                        </CancelButton>
                        <SaveButton onClick={handleSave}>Save</SaveButton>
                    </ButtonsWrapper>
                </>
            ) : (
                <DescriptionCard onClick={handleBeginEdit}>
                    {description.length > 0
                        ? description
                        : 'Add a description...'}
                </DescriptionCard>
            )}
        </>
    );
};
