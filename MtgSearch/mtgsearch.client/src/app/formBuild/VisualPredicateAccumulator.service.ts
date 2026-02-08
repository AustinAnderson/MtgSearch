import { debounceTime, Observable, Subject } from "rxjs"

export interface IAccumulableQueryFragment {
  uuid: string
  fetchFragment(): string
}
export class VisualPredicateAccumulator {
  public static escapeRegex(raw: string) {
    //using this because RegExp.escape is browser only or too new
    // Source - https://stackoverflow.com/a/5664273
    // Posted by Bart Kiers, modified by community. See post 'Timeline' for change history
    // Retrieved 2026-02-07, License - CC BY-SA 3.0
    return raw.replace(/([()[{*+.$^\\|?])/g, '\\$1');
  }
  public static Instance = new VisualPredicateAccumulator();
  public queryChangeEvents: Subject<string>;
  private componentNotifiedEvents: Observable<void>;
  private componentNotifiedEventsSender: Subject<void>;
  private constructor() {
    this.queryChangeEvents = new Subject<string>();
    this.componentNotifiedEventsSender = new Subject<void>();
    this.componentNotifiedEvents = this.componentNotifiedEventsSender.pipe(debounceTime(200));
    this.componentNotifiedEvents.subscribe(() =>
      this.queryChangeEvents.next(this.computeNewQuery())
    );
  }
  private predicates = new Map<string,IAccumulableQueryFragment>();
  public register(queryConvertable: IAccumulableQueryFragment) {
    this.predicates.set(queryConvertable.uuid,queryConvertable);
  }
  public remove(uuid: string) {
    this.predicates.delete(uuid);
  }
  public notifyChange() {
    this.componentNotifiedEventsSender.next();
  }
  private computeNewQuery() { 
    let query = Array.prototype.slice.call(this.predicates.values)
      .map(x => x.fetchFragment())
      .join(' and ');
    return query;
  }
}
