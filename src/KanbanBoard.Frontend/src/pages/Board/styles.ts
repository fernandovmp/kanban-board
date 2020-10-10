import styled from 'styled-components';
import { DefaultButton, Input, PrimaryButton } from '../../components';

export const Main = styled.main`
    display: flex;
    flex-direction: column;
    flex-grow: 1;
`;

export const BoardSection = styled.section`
    position: relative;
    flex-grow: 1;
`;

export const TaskListsWrapper = styled.div`
    display: flex;
    align-items: flex-start;
    position: absolute;
    top: 0;
    left: 0;
    bottom: 0;
    right: 0;
    overflow-x: auto;
    padding-left: 6px;
`;

export const ListWrapper = styled.div`
    padding: 8px;
`;

export const NewListButton = styled(DefaultButton)`
    box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.25);
    border-radius: 8px;
    min-width: 230px;
    padding: 4px;
`;

export const BoardTitleInput = styled(Input)`
    margin-right: 50px;
    width: fit-content;
`;

export const ButtonsWrapper = styled.div`
    display: flex;
    gap: 10px;
    justify-content: flex-end;
`;

export const CancelButton = styled(DefaultButton)`
    background: transparent;
    width: 80px;
    font-size: small;
    justify-content: center;

    &:hover {
        background: #e4e4e4;
    }
`;

export const SaveButton = styled(PrimaryButton)`
    width: 80px;
    font-size: small;
    justify-content: center;
`;
