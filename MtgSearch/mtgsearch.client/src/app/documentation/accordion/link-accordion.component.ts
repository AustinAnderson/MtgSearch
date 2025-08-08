import { Component, EventEmitter, HostBinding, Input, OnInit, Output } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  selector: "link-accordion",
  templateUrl: "./link-accordion.component.html"
})
export class LinkAccordionComponent implements OnInit {
  @Input() @HostBinding("style.--docDisplayPaneBgColor") public dispColor: string = '#FFF';
  @Input() public expanderText: string = "unknown";
  @Input() public linkNames: string[] = [];
  @Input() public changeInAccordionListener: Subject<string> = new Subject();
  ngOnInit(): void {
    this.changeInAccordionListener.subscribe(x => {
      if (x != this.expanderText) {
        this.clearAll();
      }
    });
  }

  @Output() selectionChanged = new EventEmitter<string>();

  public toggleAccordion(event: MouseEvent) {
    let element = event?.target as HTMLElement;
    if (!element) return;
    let button = element.closest('button');
    if (!button) return;
    button.classList.toggle('active');
    let children = button.parentElement?.children;
    if (!children || children.length < 2) return;
    children[1].classList.toggle('active');
  }
  //TODO: change binding model to list {name:string, active:bool}, bind [classList.active]=active?
  //change clickLink to pass in name, then just set active for that one
  private clearAll() {
    console.log("clearing all for "+this.expanderText);
  }
  public clickLink(event: MouseEvent) {
    let element = event?.target as HTMLElement;
    if (!element) return;
    let li = element.closest('li');
    if (!li) return;
    let children = li.parentElement?.children;
    if (!children || children.length < 1) return;
    for (let child of Array.prototype.slice.call(children)) {
      child.classList.remove('active');
    }
    li.classList.add('active');
    let linkName = li.firstChild?.textContent;
    if (!linkName) {
      console.log("couldn't fetch which link was clicked for list item", li);
      linkName = "__error";
    }
    this.selectionChanged.emit(linkName);
  }
}

