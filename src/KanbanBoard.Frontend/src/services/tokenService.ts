export const getJwtToken = () => sessionStorage.getItem('jwtToken') ?? '';
export const setJwtToken = (jwtToken: string) =>
    sessionStorage.setItem('jwtToken', jwtToken);
