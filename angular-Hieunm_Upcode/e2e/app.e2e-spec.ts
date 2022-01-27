import { MHPQTemplatePage } from './app.po';

describe('MHPQ App', function () {
  let page: MHPQTemplatePage;

  beforeEach(() => {
    page = new MHPQTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
