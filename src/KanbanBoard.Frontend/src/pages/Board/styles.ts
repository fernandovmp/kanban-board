import styled from 'styled-components';
import { DefaultButton, Input } from '../../components';

export const Main = styled.main`
    padding: 0 60px;
`;

export const Header = styled.header`
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    gap: 10px;
    padding: 20px 0;
`;

export const BoardTitle = styled.h2`
    margin-right: 50px;
    margin: 0 50px 0 0;
`;

export const TaskListsWrapper = styled.div`
    display: flex;
    gap: 20px;
    align-items: flex-start;
`;

export const NewListButton = styled(DefaultButton)`
    box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.25);
    border-radius: 8px;
    width: 230px;
`;

export const BoardTitleInput = styled(Input)`
    margin-right: 50px;
    width: fit-content;
`;
